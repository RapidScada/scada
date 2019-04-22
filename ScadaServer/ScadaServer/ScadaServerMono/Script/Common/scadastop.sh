#!/bin/sh
echo "Terminating Rapid SCADA..."
sudo service scadaagent stop
sudo service scadacomm stop
sudo service scadaserver stop
