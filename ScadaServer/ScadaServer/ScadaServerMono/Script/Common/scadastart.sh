#!/bin/sh
echo "Starting Rapid SCADA..."
sudo service scadaserver start
sudo service scadacomm start
sudo service scadaagent start
