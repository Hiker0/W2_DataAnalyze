   M S B A M L   `   `   `  ����I  EW2_DataAnalyze, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null/clr-namespace:W2_DataAnalyzeW2_DataAnalyze  5   	   E AWPF.Themes, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null;,clr-namespace:WPF.Themes;assembly=WPF.Themes
WPF.Themes 5   	       W2_DataAnalyze.MainWindow   5   	   W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35U QWPFToolkit, Version=3.5.40128.1, Culture=neutral, PublicKeyToken=31bf3856ad364e35F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     5   	   R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   @d2http://schemas.microsoft.com/expression/blend/2008     5   	   Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5   	   .localclr-namespace:W2_DataAnalyze     5   	   ?themes,clr-namespace:WPF.Themes;assembly=WPF.Themes     5   	     WPF.Themes.ThemeManager    Theme$  
BureauBlue��5   	    � Title$ W2数据解析��5
   
   $	��350��6   $	��600��6)   $	��350��68   $	��600��6E   ���	#FFAED1D8$��0q�6h    J� SnapsToDevicePixels .  � WindowStyle$ ThreeDBorderWindow=�6�    J� Opacity$ 1Q�6�    � Topmost .   � Icon$ watch_128X128.ico��6�    � Cursor$
 Hand��6�    J� Effect 5      3  -System.Windows.Media.Effects.DropShadowEffect  5   
   ��5      �� 5   
   	 �� EndPoint$	 0.5,1&�6   
 �� 
StartPoint$
 0.5,0&�6/   .��5      �  � Color$ 	#FFEEE0BC��6    � Offset$ 0Q�6-   � 5      $ 	#FFE6E0D2��6   $ 1Q�6-   � 5      $ 	#FFDBE6D1��6   $ 0.983Q�6-   5      .��5      ]� $��Stretch=�6   $��Stretch=�6.   $	��320��6J   ��.  ���#EEEEEE.+�5   
   ]� $��40��6   $	��Top=�6!   ��. $	��Top=�6N   
�������.+�5      �� $��	10,0,10,0q�6   $��Center=�6'   $��数据类型��6D   �� 5      -   5        $��dataType��5      $��90��6'   $��24��62    J� RenderTransformOrigin$ 5.955,0.672&�6>   .@�5      ��  �� 
IsSelected . .��61   心率65   �� 5      .��6    今日睡眠6&   �� 5      .��6   全部睡眠6%   �� 5      .��6   训练6#   �� 5      .��6   训练结果6%   �� 5      .��6   统计信息6%   �� 5       .��6   分时计步6%   5!      �� 5"      $��Center=�6   $��选择数据文件��61   $��	20,0,10,0q�6B   �� 5#      -   5        $��fielpath��5#   J   $��23��6    �� TextWrapping$
 Wrap=�6"   $�� ��66   $	��220��6>   6Z   �� 5$      -   5        $��btnWPF��5$   )   $��	10,0,10,0q�6   $��50��67   $	��200��6E   $��Center=�6T   $
��Left=�6�   ��5%      �� 5&      $��Resources/timg.png��6   $��19��68   $��41��6D   5(      5)      �� 5*   
   $	��Top=�6   $��Stretch=�6-   $
��Auto=�6I   $
��Auto=�6n   .��5+      �� -   5        $��result��5+      
���   �$��20,10q�6>   5,      5.      5/      