   M S B A M L   `   `   `  ����C  ?MVVM ERP, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null/clr-namespace:MVVM_ERP.ViewsMVVM_ERP.Views  5   	   9!clr-namespace:MVVM_ERP.ViewModelsMVVM_ERP.ViewModels  5   	   "    MVVM_ERP.Views.EmployeesView   5   	   R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5   	   R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   @d2http://schemas.microsoft.com/expression/blend/2008     5   	   Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5   	   .localclr-namespace:MVVM_ERP.Views     5   	   0vm!clr-namespace:MVVM_ERP.ViewModels     5   	   -   5          �Name$  Window��5   	    � Title$ EmployeesView��5
   	   $	��300��6   $��426.842��6,    � 	Resources 5      ,  &MVVM_ERP.ViewModels.EmployeesViewModel   employeesViewModel��%   &	          5   
   6    5      .��5      � -   5         �Name$ mainGrid��5       � DataContext# [  6    � RowDefinitions 5   
   �� 5      $��20 �6   �� 5      $��30 �6   �� 5      $��* �6   �� 5      $��30 �6   �� 5      $��25 �6   .+�5   
   �� $��1��6    �� Orientation$ 
Horizontal=�6"   $
��10,2q�6;   .+�5      �� $	��Add��6   $��60��6I   ��6#   �� *AddNewItemCommand+6I   �� 5      $y�Hidden=�6   $��10��6,   �� 5      $
��Edit��6   $��60��6P   ��6$   �� *EditSelectedItemCommand+6P   �� 5      $y�Hidden=�6   $��10��6,   �� 5      $��Delete��6   $��60��6T   ��6&   �� *DeleteSelectedItemCommand+6T   5      &   System.Windows.Controls.DataGrid  5   
   $��2��6   $��10q�6       AutoGenerateColumns .     RowHeaderWidth$ 0Q�6H   	   
IsReadOnly	 . ��5      �� *	Employees+
 �� SelectedItem
 6G   �� *SelectedEmployee+   Columns 5      0  *System.Windows.Controls.DataGridTextColumn  5       ,  &System.Windows.Controls.DataGridColumn   Header$ Username��6%      Width5  /System.Windows.Controls.DataGridLengthConverter$
 0.4* 67   1  +System.Windows.Controls.DataGridBoundColumn   Binding 6D   �� *
username+  5!      $
 Name��6%   $
 0.7* 63    6@   �� *name+  5"      $ 	Last name��6%   $
 0.7* 68    6E   �� *
lastName+5$      �� 5%   
   $��3��6   $ 
Horizontal=�6"   $��RightToLeft=�6;   $
��10,0q�6W   .+�5'      �� $
��Save��6   $��90��6K   ��6$   �� *SaveChangesCommand+��5(      ��  �� ElementName$ Window���� 5)      $��5��6   $y�Hidden=�6"   �� 5*      $��Close��6   $��90��6F   ��6%   �� *CloseCommand+��5+      �� $ Window��5,      5-      5.      