#!/bin/sh
cd /etc/init.d
sudo chmod +x ./scadaagent
sudo update-rc.d scadaagent defaults
sudo chmod +x ./scadaserver
sudo update-rc.d scadaserver defaults
sudo chmod +x ./scadacomm
sudo update-rc.d scadacomm defaults
