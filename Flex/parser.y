%{
#include <stdio.h>
#include <stdlib.h>
#include <string.h>

// Объявляем функции для работы с лексером
extern int yylex();
extern int yyparse();
extern FILE *yyin;

void yyerror(const char *s) {
    fprintf(stderr, "Error: %s\n", s);
}
%}

// Объявляем типы для значений токенов
%union {
    double num;
    char *str;
}

// Объявляем токены
%token CONST VAL COLON EQUALS SEMICOLON
%token <num> FLOAT_NUMBER
%token <str> IDENTIFIER DOUBLE FLOAT INT

// Указываем типы для нетерминалов
%type <str> Type
%type <str> Identifier
%type <num> Value

// Правила грамматики
%%

ConstDeclaration:
    CONST VAL Identifier COLON Type EQUALS Value SEMICOLON {
        printf("Valid constant declaration: %s : %s = %f\n", $3, $5, $7);
    }
    ;

Type:
    DOUBLE { $$ = strdup("Double"); }
    | FLOAT { $$ = strdup("Float"); }
    | INT { $$ = strdup("Int"); }
    ;

Identifier:
    IDENTIFIER { $$ = $1; }
    ;

Value:
    FLOAT_NUMBER { $$ = $1; }
    ;

%%

int main(int argc, char **argv) {
    if (argc > 1) {
        FILE *file = fopen(argv[1], "r");
        if (!file) {
            perror("Could not open file");
            return 1;
        }
        yyin = file;
    }

    yyparse();
    return 0;
}
