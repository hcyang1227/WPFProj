   M S B A M L   `   `   `  ����Q  MApp002_DataBindingTest, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null?$clr-namespace:App002_DataBindingTestApp002_DataBindingTest  5   	   '    !App002_DataBindingTest.MainWindow   5   	   W SPresentationCore, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35e aSystem.Windows.Controls.Ribbon, Version=8.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089R NWindowsBase, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     5   	   R NSystem.Xaml, Version=8.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   @d2http://schemas.microsoft.com/expression/blend/2008     5   	   Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5   	   6local$clr-namespace:App002_DataBindingTest     5   	     � Title$  
MainWindow��5   	   $	��200��6   $	��400��6)   .��5
      � $��Center=�6   $��Center=�6'    � RowDefinitions 5   
   �� 5      $
��Auto �6   �� 5      $
��Auto �6   �� 5      $��* �6    � ColumnDefinitions 5   
   �� 5      $
��Auto �6   �� 5      $	��200 �6   .+�5   
   �� -   5        $��lblName��5      $��2q�6   .��6*   _Name:62   �� 5   
   -   5        $��txtName��5      $��1��6!   $��2q�61   ��6<   �� *Name+ �� Mode$ TwoWay=��� 5   
   -   5        $��lblAge��5      $��2q�6   $��1��6)   .��66   _Age:6=   �� 5   
   -   5        $��txtAge��5      $��1��6    $��1��6-   $��2q�6=   ��6H   �� *Age+$ TwoWay=��� 5   
   $��2��6   $��2��6"   .+�5       �� -   5        $��_Show...��5       6(   5!      5#      5%      