#include "lexer_parser.h"
#include "parser.tab.h"
#include <stdio.h>
#include <stdlib.h>

// Внешние функции, сгенерированные FLEX и BISON
extern int yyparse();
extern FILE* yyin;

// Функция для анализа строки
void parse_string(const char* input) {
    // Открываем строку как файловый поток
    yyin = fmemopen((void*)input, strlen(input), "r");
    if (!yyin) {
        perror("Failed to open input string");
        return;
    }

    // Запускаем парсер
    yyparse();

    // Закрываем поток
    fclose(yyin);
}
