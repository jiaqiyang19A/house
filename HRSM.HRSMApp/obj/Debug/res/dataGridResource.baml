   M S B A M L   `   `   `  ����W  SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089  �� 
TargetType   textColStyleCenter �� Property �� Value  textColStyleLeft  chkColStyle)   #System.Windows.Controls.DataGridRow  
dgRowStyle �� Triggers P� Property P� Value �� EndPoint �� 
StartPoint � Color	 � Offset  colStyle=  7System.Windows.Controls.Primitives.DataGridColumnHeader  colDarkBlueStyle  dgRowDarkBlueStyle*  $System.Windows.Controls.DataGridCell  dgcellStyle  itemBtnStyle
 �� 
TargetType ��Name �� CornerRadius �� Triggers �� 
TargetName �� Value �� Binding �� RelativeSource �� Mode �� Path&   System.Windows.Controls.DataGrid 	 dgStyle 
 dgDarkBlueStyle  textColStyleLeft1�� 5      D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     :x,http://schemas.microsoft.com/winfx/2006/xaml      5      %X  &	        &	 �     &	 /    &	 �    &	 �    &	 l    &	     &	 �    &	 	    &		 �    7  7  7  &	
     7  7  &	     �� 5      "  ��6   .�5   
   ��  � ��$ Center=�6.   �� 5   
    �@��TextAlignment$ Center=�6*   5      �� 5      "  ��6   .�5   
   ��  � ��$ Center=�6.   �� 5	   
    �@��TextAlignment$
 Left=�6*   �� 5
   
    �@��Padding$	 5,0q�6$   5      �� 5      "  ��6   .�5   
   ��  � ��$ Center=�6.   �� 5   
    � ��$ Center=�60   5      �� 5      "    6   .�5   
   ��  � ��
 ���� 6'   �� 5   
    � ��$ Center=�65    5   
   P� 5       �@��AlternationIndex$ 0��6?   . �5      ��  � �� �	#FFD5EFF76/   5      P� 5       �@��AlternationIndex$ 1��6?   . �5      ��  � �� �	#FFFBFCF96/   5      P� 5       �@J�IsMouseOver . . �5      ��  � �� 5      �� 5      $ 0.5,1&�6.   $ 0.5,0&�6?   .��5      � $ 	#FF73BCE8��6+   $
	 0.98Q�6=   � 5      $ White��6+   $	 0Q�69   5       5"      5#      5&      �� 5'      "   6   .�5(   
   ��  � ��$
 Bold"�6'   �� 5)   
    � �� �	#FF7C6BE06'   5*      �� 5-   
   "   6*   .�5.      ��  � ��$
 Bold"�6/   �� 5/       � ��$ 25��6+   �� 50       � ��$
 20,0q�6,   �� 51       � ��
 �����6/   �� 52       � �� 53      �� 54   "   $ 0.5,0&�66   $ 0.5,1&�6I   .��55   *   � $ #5A80B9��67   $	 0Q�6G   � 56   *   $ #294E89��67   $		 0.4Q�6G   � 57   *   $ #5A80B9��67   $	 1Q�6G   58   #   5:      5;      �� 5=   
   "    6   .�5>      ��  � ��
 ���� 6/   �� 5?       � ��$ Center=�6=    5@      P� 5A       �@J�IsMouseOver . . �5B   "   ��  � �� 5C   *   �� 5D   2   $ 0.5,1&�6F   $ 0.5,0&�6W   .��5E   :   � $ #5A80B9��6G   $
	 0.98Q�6W   � 5F   :   $ White��6G   $	 0Q�6U   5G   3   5I   #   5J      5L      �� 5N      "   6    5O   
   P� 5P       �@ 
IsSelected . . �5Q      ��  � ��$ 0q�64   5R      P� 5S       �@ 
IsSelected . . �5T      ��  � �� 5U      �� 5V      $ 0.5,1&�6.   $ 0.5,0&�6?   .��5W      � $ 	#FF13418E��6+   $
	 0.98Q�6=   � 5X      $ White��6+   $	 0Q�69   5Y      5[      5\      5^      �� 5`   
   "  ��6   .�5a   
   ��  � ��$ 60��6"   �� 5b   
    � ��$ 25��6#   �� 5c   
    � ��
 �����6'   �� 5d   
    � �� 5e      �� 5f      "
 ��6"   .R�5g      �� $ bd��6   $��1q�6)   
����ح�#��z��6U   $ 3��6   ._�5h      �� $��Center=�6+   $��Center=�6H   5i       5j      P� 5k       �@J�IsMouseOver . . �5l      �� $ bd��6%    � �� �	#FFA2C6C76K   �� 5m       � ��
 �����6<   5n      �� 5o      $ 添加子项��6q    6&   ��  �� $
 Self=�$ Content�.a�5p      ��  � �� �	#FFF48D496;   5q      �� 5r      $ 修改��6q    6&   ��  �� $
 Self=�$ Content�.a�5s      ��  � ��
 ��  �6;   5t      �� 5u      $ 删除��6q    6&   ��  �� $
 Self=�$ Content�.a�5v      ��  � ��
 �  ��6;   5w      �� 5x      $ 恢复��6q    6&   ��  �� $
 Self=�$ Content�.a�5y      ��  � �� �	#FF3876B96;   5z      �� 5{      $ 移除��6q    6&   ��  �� $
 Self=�$ Content�.a�5|      ��  � �� �	#FF5952526;   5}      5      5�      5�      �� 5�      "   6   .�5�   
   ��  � ��$ Stretch=�60   �� 5�   
    �@ AutoGenerateColumns .  60   �� 5�   
    �@ SelectionMode$ Extended=�6*   �� 5�   
    � ��$ Stretch=�6.   �� 5�   
    �@ CanUserAddRows .  6+   �� 5�   
    �@ RowHeaderWidth$ 20Q�6+   �� 5�   
    �@ HeadersVisibility$ Column=�6.   �� 5�   
    �@��AlternationCount$ 2��6-   �� 5�   
   % �@ HorizontalScrollBarVisibility$
 Auto=�6:   �� 5�   
   # �@ VerticalScrollBarVisibility$
 Auto=�68   �� 5�   
    �@ SelectionUnit$ FullRow=�6*   �� 5�   
    �@ ColumnHeaderHeight$ 25Q�6/   �� 5�   
    �@ 	RowHeight$ 25Q�6&   �� 5�   
     �@ HorizontalGridLinesBrush
 �����65   �� 5�   
    �@ VerticalGridLinesBrush
 �����63   �� 5�   
    �@ ColumnHeaderStyle8   6.   �� 5�   
    � ��$ 5,20,0,5q�6#   �� 5�   
    � ��
 ���� 6'   �� 5�   
    � ��
 �����6(   �� 5�   
    �@ RowStyle8  6%   �� 5�   
    �@ 	CellStyle8  6&   5�      �� 5�   
   "   6   .�5�      ��  � ��$ Stretch=�68   �� 5�       �@ AutoGenerateColumns .  68   �� 5�       �@ SelectionMode$ Extended=�62   �� 5�       � ��$ Stretch=�66   �� 5�       �@ CanUserAddRows .  63   �� 5�       �@ RowHeaderWidth$ 20Q�63   �� 5�       �@ HeadersVisibility$ Column=�66   �� 5�       �@��AlternationCount$ 2��65   �� 5�      % �@ HorizontalScrollBarVisibility$
 Auto=�6B   �� 5�      # �@ VerticalScrollBarVisibility$
 Auto=�6@   �� 5�       �@ SelectionUnit$ FullRow=�62   �� 5�       �@ ColumnHeaderHeight$ 25Q�67   �� 5�       �@ 	RowHeight$ 25Q�6.   �� 5�        �@ HorizontalGridLinesBrush �	#FF8EB0D46=   �� 5�       �@ VerticalGridLinesBrush �	#FF8EB0D46;   �� 5�       �@ ColumnHeaderStyle8   66   �� 5�       � ��$ 5,20,0,5q�6+   �� 5�       � ��
 �����6/   �� 5�       �@ RowStyle8  6-   5�      �� 5�   
   "  ��6   .�5�      ��  � ��$ Center=�66   �� 5�       �@��TextAlignment$
 Left=�62   �� 5�       �@��Padding$
 20,0q�6,   5�      5�      