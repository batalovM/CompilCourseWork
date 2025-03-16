%{
#include "parser.tab.h"  // Подключаем заголовочный файл, сгенерированный BISON
%}

%%

"const"         { return CONST; }
"val"           { return VAL; }
"Double"        { return DOUBLE; }
"Float"         { return FLOAT; }
"Int"           { return INT; }
[a-zA-Z_][a-zA-Z0-9_]*  { yylval.str = strdup(yytext); return IDENTIFIER; }
[0-9]+\.[0-9]+  { yylval.num = atof(yytext); return FLOAT_NUMBER; }
":"             { return COLON; }
"="             { return EQUALS; }
";"             { return SEMICOLON; }
[ \t\n]         ;  // Игнорируем пробелы, табуляции и переводы строк
.               { printf("Unknown character: %s\n", yytext); }

%%

int yywrap() {
    return 1;
}
