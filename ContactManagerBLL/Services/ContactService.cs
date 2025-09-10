using System.Globalization;
using AutoMapper;
using ContactManagerBLL.Interfaces;
using ContactManagerBLL.Models;
using ContactManagerDAL.Entities;
using ContactManagerDAL.Interfaces;
using CsvHelper;
using CsvHelper.Configuration;

namespace ContactManagerBLL.Services;

public class ContactService(IContactRepository contactRepository, IMapper mapper) : IContactService
{
    public async Task<IEnumerable<ContactModel>> GetAllAsync()
    {
        var entities = await contactRepository.GetAllAsync();
        return mapper.Map<IEnumerable<ContactModel>>(entities);
    }

    public async Task<ContactModel?> GetByIdAsync(int id)
    {
        var entity = await contactRepository.GetByIdAsync(id);
        return entity is null ? null : mapper.Map<ContactModel>(entity);
    }

    public async Task AddAsync(ContactModel model)
    {
        var entity = mapper.Map<Contact>(model);
        await contactRepository.AddAsync(entity);
    }

    public async Task UpdateAsync(ContactModel model)
    {
        var entity = mapper.Map<Contact>(model);
        await contactRepository.UpdateAsync(entity);
    }

    public async Task DeleteAsync(int id)
    {
        await contactRepository.DeleteAsync(id);
    }

    public async Task<(int imported, List<string> errors)> ImportCsvAsync(Stream csvStream, CancellationToken ct = default)
    {
        var errors = new List<string>();
        var toInsert = new List<Contact>();

        // Конфиг CSV
        var config = new CsvConfiguration(CultureInfo.InvariantCulture)
        {
            HasHeaderRecord = true,
            TrimOptions = TrimOptions.Trim,
            BadDataFound = null,
            MissingFieldFound = null,
            DetectDelimiter = true,
        };

        using var reader = new StreamReader(csvStream);
        using var csv = new CsvReader(reader, config);

        await csv.ReadAsync();
        csv.ReadHeader();

        string[] required = ["Name", "DateOfBirth", "Married", "Phone", "Salary"];
        foreach (var req in required){
            if (!csv.HeaderRecord.Any(h => string.Equals(h, req, StringComparison.OrdinalIgnoreCase)))
                errors.Add($"Missing required column: {req}");
        }
        if (errors.Count > 0) return (0, errors);

        var row = 1;
        while (await csv.ReadAsync()){
            row++;
            try{
                string Get(string name) => csv.GetField(csv.HeaderRecord.First(h => string.Equals(h, name, StringComparison.OrdinalIgnoreCase)));

                var name = Get("Name");
                var dobStr = Get("DateOfBirth");
                var marriedStr = Get("Married");
                var phone = Get("Phone");
                var salaryStr = Get("Salary");

                if (string.IsNullOrWhiteSpace(name)){
                    errors.Add($"Row {row}: Name is required");
                    continue;
                }
                if (string.IsNullOrWhiteSpace(phone)){
                    errors.Add($"Row {row}: Phone is required");
                    continue;
                }

                if (!DateTime.TryParse(dobStr, CultureInfo.InvariantCulture, DateTimeStyles.None, out var dob)){
                    if (!DateTime.TryParse(dobStr, out dob)){
                        errors.Add($"Row {row}: invalid DateOfBirth '{dobStr}'");
                        continue;
                    }
                }

                bool married;
                var m = marriedStr.Trim().ToLowerInvariant();
                if (m is "true" or "1" or "yes" or "y") married = true;
                else if (m is "false" or "0" or "no" or "n") married = false;
                else{
                    errors.Add($"Row {row}: invalid Married '{marriedStr}'");
                    continue;
                }

                if (!decimal.TryParse(salaryStr, NumberStyles.Any, CultureInfo.InvariantCulture, out var salary)){
                    if (!decimal.TryParse(salaryStr, out salary)){
                        errors.Add($"Row {row}: invalid Salary '{salaryStr}'");
                        continue;
                    }
                }

                toInsert.Add(new Contact
                {
                    Name = name,
                    DateOfBirth = dob,
                    Married = married,
                    Phone = phone,
                    Salary = salary
                });
            }
            catch (Exception ex){
                errors.Add($"Row {row}: {ex.Message}");
            }
        }

        if (toInsert.Count > 0)
            await contactRepository.AddRangeAsync(toInsert);
        return (toInsert.Count, errors);
    }
}