import { Component, OnInit } from '@angular/core';
import { Input } from '@angular/core';
import { Output, EventEmitter } from '@angular/core';

@Component({
    selector: 'app-todo',
    templateUrl: './todo.component.html',
    styleUrls: ['./todo.component.css']
})
export class TodoComponent implements OnInit {
    @Input() todo;
    @Output() delete = new EventEmitter();

    constructor() { }

    ngOnInit() {
    }

    onDeleteClick() {
        this.delete.emit(this.todo);
    }
}
