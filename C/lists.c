#include <stdio.h>
#include <stdlib.h>

typedef struct node {
    int data;
    struct node * next;
} node_t;

node_t * reverse(node_t * head) {
    node_t *a = head, *b = head->next;
    while(a->next)
    {
        b = a->next;
        a->next = a->next->next;
        b->next = head;
        head = b;
    }
    return head;
}

void print_list(node_t * head) {
    node_t * current = head;

    while (current) {
        printf("%d\n", current->data);
        current = current->next;
    }
}

void append(node_t * head, int data) {
    node_t * current = head;
    while (current->next) {
        current = current->next;
    }

    /* add a new item to the end of the list */
    current->next = malloc(sizeof(node_t));
    current->next->data = data;
    current->next->next = NULL;
}

void delete_list(node_t *head) {
    node_t  *current = head, 
            *next = head;

    while (current) {
        next = current->next;
        free(current);
        current = next;
    }
}

int main(int argc, char **argv) 
{
    node_t * head = NULL;
    head = malloc(sizeof(node_t));
    if (head == NULL) {
        return 1;
    }

    // add tail
    head->data = 1;
    head->next = NULL;

    append(head, 2);
    append(head, 3);
    append(head, 4);
    append(head, 5);
    append(head, 6);

    puts("initial:");
    print_list(head);

    head = reverse(head);

    puts("reversed:");

    print_list(head);

    delete_list(head);

	return 0;
}
