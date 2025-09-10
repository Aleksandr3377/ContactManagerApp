import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { Contact, ContactService } from '../../services/contact.service';

@Component({
  selector: 'app-contact-list',
  standalone: true,
  imports: [CommonModule, FormsModule],
  templateUrl: './contact-list.component.html',
  styleUrls: ['./contact-list.component.css']
})
export class ContactListComponent implements OnInit {
  contacts: Contact[] = [];
  filterText: string = '';

  sortColumn: keyof Contact | null = null;
  sortAsc: boolean = true;

  editingId: number | null = null;
  editedContact: Contact | null = null;

  constructor(private contactService: ContactService) {}

  ngOnInit(): void {
    this.loadContacts();
  }

  loadContacts() {
    this.contactService.getAll().subscribe(data => {
      this.contacts = data;
    });
  }

  deleteContact(id: number) {
    this.contactService.delete(id).subscribe(() => {
      this.contacts = this.contacts.filter(c => c.id !== id);
    });
  }

  startEdit(contact: Contact) {
    this.editingId = contact.id;
    this.editedContact = { ...contact };
  }

  cancelEdit() {
    this.editingId = null;
    this.editedContact = null;
  }

  saveEdit() {
    if (this.editedContact) {
      this.contactService.update(this.editedContact).subscribe(() => {
        this.contacts = this.contacts.map(c =>
            c.id === this.editedContact!.id ? this.editedContact! : c
        );
        this.cancelEdit();
      });
    }
  }


  get filteredContacts(): Contact[] {
    let result = [...this.contacts];

    if (this.filterText) {
      result = result.filter(c =>
          c.name.toLowerCase().includes(this.filterText.toLowerCase()) ||
          c.phone.toLowerCase().includes(this.filterText.toLowerCase())
      );
    }

    if (this.sortColumn) {
      result.sort((a, b) => {
        const valA = a[this.sortColumn!] as any;
        const valB = b[this.sortColumn!] as any;

        if (valA < valB) return this.sortAsc ? -1 : 1;
        if (valA > valB) return this.sortAsc ? 1 : -1;
        return 0;
      });
    }

    return result;
  }

  sort(column: keyof Contact) {
    if (this.sortColumn === column) {
      this.sortAsc = !this.sortAsc;
    } else {
      this.sortColumn = column;
      this.sortAsc = true;
    }
  }

  getSortIcon(column: keyof Contact): string {
    if (this.sortColumn !== column) return '';
    return this.sortAsc ? '↑' : '↓';
  }
}
