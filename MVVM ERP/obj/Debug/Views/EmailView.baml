   M S B A M L   `   `   `  ����C  ?MVVM ERP, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null/clr-namespace:MVVM_ERP.ViewsMVVM_ERP.Views  5   	   9!clr-namespace:MVVM_ERP.ViewModelsMVVM_ERP.ViewModels  5   	       MVVM_ERP.Views.EmailView   5   	   R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5   	   R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   @d2http://schemas.microsoft.com/expression/blend/2008     5   	   Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5   	   .localclr-namespace:MVVM_ERP.Views     5   	   0vm!clr-namespace:MVVM_ERP.ViewModels     5   	   -   5          �Name$  Window��5   	    � Title$ 	EmailView��5
   	   $	��300��6   $	��300��6(    � 	Resources 5      (  "MVVM_ERP.ViewModels.EmailViewModel   emailViewModel��%   &	          5   
   6   5      .��5      � -   5         �Name$ mainGrid��5       � DataContext# [  6    � ColumnDefinitions 5   
   �� 5      $
��0.3* �6   �� 5      $
��0.7* �6    � RowDefinitions 5   
   �� 5      $��25 �6   �� 5      $��10 �6   �� 5      $��* �6   �� 5      $��35 �6   .+�5   
   �� $	��To:��6   $��Right=�6   �� 5   
   $��0��6   $��1��6   ��6-   �� *Client.email+�� 5   
   $��2��6   $��2��6   $��5q�63    �� TextWrapping$
 Wrap=�5       �� AcceptsReturn . ��5   >   �� *	EmailBody+5   &   �� 5   
   $��1��6   $��5��6!   $��2��6,   �� 5   
   $��3��6   $��2��6"   	 �� Orientation$	 
Horizontal=�66   $��RightToLeft=�6O   $	��0,5q�5      .+�5       �� $y�Hidden=�6   $��5��6,   �� 5!      $
��Send��6   $��80��6$   ��6/   �� *SendEmailCommand+�� 5"      $y�Hidden=�6   $��10��6,   �� 5#      $��Cancel��6   $��80��6&   ��61   �� *CancelCommand+��6S   �� 
 �� ElementName$
 Window��5$      5%      5&      