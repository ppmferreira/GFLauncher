   M S B A M L   `   `   `  ����E  AGFLauncher, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null'clr-namespace:GFLauncher
GFLauncher  5   	   %    GFLauncher.AccountManagerWindow   5   	   W SPresentationCore, Version=6.0.2.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=6.0.2.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35e aSystem.Windows.Controls.Ribbon, Version=6.0.2.0, Culture=neutral, PublicKeyToken=b77a5c561934e089R NWindowsBase, Version=6.0.2.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35^ ZWindowsFormsIntegration, Version=6.0.2.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35H 9http://schemas.microsoft.com/winfx/2006/xaml/presentation      5   	   R NSystem.Xaml, Version=6.0.2.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   @d2http://schemas.microsoft.com/expression/blend/2008     5   	   Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5   	   *localclr-namespace:GFLauncher     5   	     � Title$  Gerenciar Contas��5   	   $	��450��6"   $	��400��6/    � WindowStyle$
 None=�6;    � AllowsTransparency$
 True��6N    � WindowStartupLocation$ CenterScreen=�6h   ���#2C3E50.��5	      � .+�5   
   � -   5        $��TitleBar��5      $��40��6   ���#34495E$	��Top=�6@   .+�5      �� -   5        $��X��5      $��30��6!   $��30��6,   $��Right=�68   $	��Top=�6T   $��5q�6l   
������ 
������ $��16(�5   _   
�������6m   5      �� 5   
   $	��Top=�6   $��Center=�6-   $��0,45,0,0q�6J   .+�5      �� $��Login:��6   
�������$��14(�69   �� 5      -   5         ��Name$ txtLogin��5      $	��250��6(   $��35��64   $��5q�6@   ���#3498DB$��1q�6b   $��14(�6v   ���#ECF0F16�   �� 5      $��Senha:��6   
�������$��14(�69   G� 5      -   5         G�Name$ txtPassword��5      $	��250��6/   $��35��6;   $	��5,5q�6G   $��5q�6T   ���#3498DB$��1q�6v   $��14(�6�   $��Center=�6�   ���#ECF0F16�   �� 5       �� Orientation$ 
Horizontal=�6   .+�5      �� -   5         ��Name$ btnAddAccount��5      $��Adicionar Conta��60   $	��100��6J   $��35��6V   $��5q�6b   ���#3498DB
�������
������ 6�   �� 5      -   5        $ btnRemoveAccount��5      $��Remover Conta��63   $	��100��6K   $��35��6W   $��5q�6c   ���#E74C3C
�������
������ 6�   �� 5      -   5        $ btnSelectAccount��5      $��Selecionar Conta��63   $	��100��6N   $��35��6Z   $��5q�6f   ���#2ECC71
�������
������ 6�   5      �� 5"      -   5         ��Name$ txtSelectedAccount��5"      
�������$��14(�6G   $
��5,10q�6U   $��Center=�6c   �� 5#      -	   5        	 ��Name$	 lstAccounts��5#      $	��178��6+   $	��300��68   ���#ECF0F1���#3498DB$��1q�6o   $��14(�6�   5(      5)      5*      