   M S B A M L   `   `   `  ����M  IZarysManagment2018, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null7 clr-namespace:ZarysManagment2018ZarysManagment2018  5   	   #    ZarysManagment2018.MainWindow   5   	   R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5   	   R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   @d2http://schemas.microsoft.com/expression/blend/2008     5   	   Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5   	   2local clr-namespace:ZarysManagment2018     5   	     � Title$  
MainWindow��5   	   $	��800��6   $
��1000��6)    � 	Resources 5
         DGHeaderStyle �� 
TargetType&   System.Windows.Controls.DataGrid �� Property  DGCHeaderStyle �� Value=  7System.Windows.Controls.Primitives.DataGridColumnHeader 
 panele  panele2��%@  &	        &	 X     &	 �    &	 �    �� 5   
   "  6&   .�5      ��  �@ ColumnHeaderStyle# �  6;   5      �� 5   
   "  6'   .�5      ��  � ��$ 30��68   �� 5       � ��
 �����63   �� 5       � ��
 �   �63   �� 5       � ��$ 14(�61   �� 5       � ��$ Calibri)�63   �� 5       � ��$	 0.5q�68   �� 5       � ��
 �   �64   5      �� 5   
   " ]�6   .�5      ��  � ��
 �  ��61   5      �� 5   
   " ��6    .�5      ��  � ��
 �  ��61   5      5      .��5!      �� -   5        $��tabcontrol1��5!   @   .@�5"   
   �� $
��Bazy��6   .��5#      � .+�5%        -   5        $��datagridZam��5%   K   $
��Left=�6^   $	��400��5&      $��314,271,0,0q�6    $	��Top=�65   $	��400��6M      AutoGenerateColumns .     Columns 5'      0  *System.Windows.Controls.DataGridTextColumn  5(      ,  &System.Windows.Controls.DataGridColumn   Header$ Data zamówienia��6-   1  +System.Windows.Controls.DataGridBoundColumn   Binding 6F   �� *czas_utworzenia+	 �� StringFormat$	 
yyyy-MM-dd��  5)      $ Nabywca��6-   
   Width5  /System.Windows.Controls.DataGridLengthConverter$
 2* 6>      CanUserResize .   6_   �� *	nabywca+5+      �� 5,      -   5        $��Dodaj Zamowienie��5,   .   $
��Left=�6I   $��
345,88,0,0q�6d   $	��Top=�5-      $	��125��61   5.      5/      �� 50   
   -   5        $��tabitem2��50   '   $��
Zamowienie��6   $	��200��67   53      �� 54   
   $��Zarzadzaj bazami��6   .��55      � .+�56      �� -   5        $��Zarządzaj bazą XML��56   (   $
��Left=�6E   $��
345,33,0,0q�6`   $	��Top=�57   ?   $	��125��6W   �� 59      -   5        $��Utworz baze sprzedazy��59   ,   $
��Left=�6L   $��204,158,0,0q�5:      $	��Top=�6&   $��75��6>   {�.  6I   �� 5;      -   5        $��Edytuj baze sprzedazy��5;   ,   $
��Left=�6L   $��394,180,0,0q�5<      $	��Top=�6&   $��75��6>   {�.  6I   �� 5=      -   5        $��Baza Klientow��5=   5   $
��Left=�6M   $��
40,110,0,0q�6h   $	��Top=�5>      $��95��66   {�.  6A   5?      5@      5A      5C      