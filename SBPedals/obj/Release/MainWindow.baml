   M S B A M L   `   `   `  ���� C  ?SBPedals, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null#clr-namespace:SBPedalsSBPedals      SBPedals.MainWindow   \ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     @d2http://schemas.microsoft.com/expression/blend/2008     Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     (localclr-namespace:SBPedals     -     � Title$'  !Steel Battalion Pedals Controller��$��264.057��$��426.848�� � 
ResizeMode$ CanMinimize=�.��� $
��Left=�$	��504�� � ColumnDefinitions �� $	��31* ��� $	��53* �.+��� -    ��Name$ txtGas��$
��Left=�$��23�� �� TextWrapping$
 Wrap=�$	��Top=�$	��120�� �� 
IsReadOnly . $��	10,86,0,0q��� -    ��Name$ lblGas��$��	Gas Value��$
��Left=�$	��Top=�$��	10,60,0,0q��� -   $ txtBrake��$
��Left=�$��23��$
 Wrap=�$	��Top=�$	��120�� . $��
10,135,0,0q��� -   $ lblBrake��$��Brake Value��$
��Left=�$	��Top=�$��
10,109,0,0q��� -   $ 	lblClutch��$��Clutch Value��$
��Left=�$��
10,158,0,0q�$	��Top=��� -   $ 	txtClutch��$
��Left=�$��23��$��
10,184,0,0q�$
 Wrap=�$	��Top=�$	��120�� . �� -   $ 	lblSerial��$��Serial Data��$
��Left=�$	��Top=�$��	10,10,0,0q��� -	   $ 	txtSerial��$
��Left=�$��23��$��	10,36,0,0q�$
 Wrap=�$	��Top=�$	��120�� . �� -
   $ 	txtGasKey��$
��Left=�$��23��$��
186,86,0,0q�$
 Wrap=�$	��Top=�$	��120�� � Cursor$ Arrow�� �� 	MaxLength$ 1��$��2���� -   $ 	lblGasKey��$��Gas Key��$
��Left=�$��
186,60,0,0q�$	��Top=�$��2���� -   $ txtBrakeKey��$
��Left=�$��23��$��186,135,0,0q�$
 Wrap=�$	��Top=�$	��120��$ Arrow��$ 1��$��2���� -   $ lblBrakeKey��$��	Brake Key��$
��Left=�$��186,109,0,0q�$	��Top=�$��2���� -   $ txtClutchKey��$
��Left=�$��23��$��186,184,0,0q�$
 Wrap=�$	��Top=�$	��120��$ Arrow��$ 1��$��2���� -   $ lblClutchKey��$��
Clutch Key��$
��Left=�$��186,158,0,0q�$	��Top=�$��2���� -   	 ��Name$	 cmbPorts��$
��Left=�$��0,36,0,0q�$	��Top=�$	��120��$��1���� -   $ 
lblComPort��$��COM Port��$
��Left=�$��0,10,0,0q�$	��Top=�$��1���� -   
 ��Name$
 
btnRefresh��$��Refresh Ports��$
��Left=�$��
139,36,0,0q�$	��Top=�$��75��$��1��