import {Component, OnInit} from '@angular/core';
import * as uuid from 'uuid';

import {TODOS} from '../todos';

@Component({
    selector: 'app-todo-list',
    templateUrl: './todo-list.component.html',
    styleUrls: ['./todo-list.component.css']
})
export class TodoListComponent implements OnInit {
    todos = TODOS;

    constructor() {
    }

    ngOnInit() {
    }

    createTodo(newTodoName) {
        if (!newTodoName) {
            return;
        }

        const todoId = uuid.v4();
        this.todos.push({
            id: todoId,
            name: newTodoName
        });
    }

    deleteTodo(itemToDelete) {
        const todoIdx = this.todos.findIndex((todo) => todo.id === itemToDelete.id);
        if (todoIdx > -1) {
            this.todos.splice(todoIdx, 1);
        }
    }
}
