   M S B A M L   `   `   `  ����   CompilCourseWork!    CompilCourseWork.MainWindow   W SPresentationCore, Version=7.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=7.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35e aSystem.Windows.Controls.Ribbon, Version=7.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089R NWindowsBase, Version=7.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     R NSystem.Xaml, Version=7.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     @d2http://schemas.microsoft.com/expression/blend/2008     Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     -     � Title$:  4Курсовая работа Компиляторы��$	��450��$	��800��.��]� .+�o� $	��Top=�.@�m� $��Файл��.@�m� -   $��Создать��m� -   $��Открыть��m� -   $��Сохранить��m� -   $��Сохранить как��m� -   $��
Выход��m� $��Правка��.@�m� -   $��Отменить��m� -   $��Повторить��m� -	   $��Вырезать��m� -
   $��Копировать��m� -   $��Вставить��m� -   $��Удалить��m� -   $��Выделить все��m� $��
Текст��.@�m� -   $'��!Постановка задачи��m� -   $��Грамматика��m� -   $5��/Классификация грамматики��m� -   $��Метод анализа��m� -   $G��AДиагностика и нейтрализация ошибок��m� -   $#��Тестовый пример��m� -   $'��!Список литературы��m� -   $0��*Исходный код программы��m� -   $��Пуск��m� $��Справка��.@�m� -   $��Вызов справки��m� -   $��О программе���� $	��Top=� �� Orientation$ 
Horizontal=�$
��Left=�$��5q�.+��� -    ��Name$ Creation��$��5q�$��40��$��40��
�������.���� $��resources\create.png���� -   $
 Open��$��5q�$��40��$��40��
�������.���� $��resources\open.png���� -   $
 Save��$��5q�$��40��$��40��
�������.���� $��resources\save.png���� -   $ Cancel��$��50, 5, 5, 5q�$��40��$��40��
�������.���� $��resources\cancel.png���� -   $ Return��$��5q�$��40��$��40��
�������.���� $��resources\return.png���� -   $
 Copy��$��5q�$��40��$��40��
�������.���� $��resources\copy.png���� -   $	 Cut��$��5q�$��40��$��40��
�������.���� $��resources\cut.png���� -    $ Paste��$��5q�$��40��$��40��
�������.���� $��resources\paste.png���� -!   $ Launch��$��5q�$��40��$��40��
�������.���� $��resources\play.png���� -"   $ Spravka��$��5q�$��40��$��40��
�������.���� $��resources\spravka.png���� -#   $
 Info��$��5q�$��40��$��40��
�������.���� $��resources\info.png��� $��5q� � RowDefinitions �� $��40 ��� $��* ��� $��* �.+��� $��0��$ ��Размер текста:��$��Center=�$��5q��� -$    ��Name$ FontSizeComboBox��$��0��$	��100��$��20��$
��Left=�$��100, 0, 0, 0q�.@��� $��10���� $��12���� $��14���� $��16���� $��18���� $��20���� $��24��� $��1��$��5q� � ColumnDefinitions �� $��45 ��� $��* �.+��� -%   $��LineNumberScroller��$��Hidden=�$��Disabled=�.���� -&    ��Name$ LineNumbers�� �� 
IsReadOnly$
 True�� �� TextWrapping$ NoWrap=�
�������$��14(��� -'   	 ��Name$	 
InputFirst��$��1��$��0q�
���   �$��1q�$��14(�"
 �� VerticalScrollBarVisibility$

 Auto=� � 	Resources  �� 
TargetType �� Property �� Value��%3   'J�       �� " J�.���  �@��Margin$ 0q��� -(   $��LineNumberScrolle1r��$��2��$��Hidden=�$��Disabled=�.���� -)   $	 InputSecond��$��2��$��5q�
���   �${�False��$��1q�