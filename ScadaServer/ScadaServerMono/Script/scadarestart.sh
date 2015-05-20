#!/bin/sh
echo "Restarting Rapid SCADA..."
sudo service scadacomm stop
sudo service scadasrv stop
sudo service scadasrv start
sudo service scadacomm start