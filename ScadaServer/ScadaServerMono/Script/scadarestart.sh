#!/bin/sh
echo "Restarting Rapid SCADA..."
sudo service scadacomm stop
sudo service scadaserver stop
sudo service scadaserver start
sudo service scadacomm start