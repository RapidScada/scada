#!/bin/sh
cd /etc/init.d
sudo chmod +x ./scadaserver
sudo update-rc.d scadaserver defaults
sudo chmod +x ./scadacomm
sudo update-rc.d scadacomm defaults