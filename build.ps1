#!/usr/bin/pwsh

write-host -ForegroundColor Magenta "Stopping Shift Dashboard..."
systemctl stop kestrel-shift-dashboard.service

cd /var/www/shift-dashboard/
pkill shift-dashboard

write-host -ForegroundColor Green "Fetching Latest version from github..."
git pull

write-host -ForegroundColor Green "Building new version of Shift Dashboard..."
dotnet build
chown -R www-data:www-data /var/www/shift-dashboard

write-host -ForegroundColor Magenta "Restarting Shift Dashboard..."
systemctl start kestrel-shift-dashboard.service

write-host -ForegroundColor Magenta "Completed."