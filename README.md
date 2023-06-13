# Arkumida
Furtails.pw next generation

# Build and run on Docker

# Infrastructure

Build image: $ docker build -f dockerfile-arkumida-infrastructure -t arkumida-infrastructure-postgres .

# WebAPI image

docker build -f dockerfile-webapi -t arkumida-webapi .

# Configure reverse proxy for API server
Configure reverse proxy in a such way (example for Apache):

##############################################################################
#                           api.arkumida.furtails.pw                         #
##############################################################################
<VirtualHost *:80>
    ServerName api.arkumida.furtails.pw
    ServerAdmin whitefossa@protonmail.com
    ErrorLog "/webroot/vhosts/api.arkumida.furtails.pw/logs/error.log"
    CustomLog "/webroot/vhosts/api.arkumida.furtails.pw/logs/access.log" combined

    ProxyPass / http://127.0.0.1:5220/
    ProxyPassReverse / http://127.0.0.1:5220/
    ProxyRequests Off
</VirtualHost>

<VirtualHost *:443>
    ServerName api.arkumida.furtails.pw
    ServerAdmin whitefossa@protonmail.com
    ErrorLog "/webroot/vhosts/api.arkumida.furtails.pw/logs/error.log"
    CustomLog "/webroot/vhosts/api.arkumida.furtails.pw/logs/access.log" combined

    ProxyPass / http://127.0.0.1:5220/
    ProxyPassReverse / http://127.0.0.1:5220/
    ProxyRequests Off

	SSLEngine on
	SSLCertificateFile "/etc/letsencrypt/live/api.arkumida.furtails.pw/cert.pem"
	SSLCertificateKeyFile "/etc/letsencrypt/live/api.arkumida.furtails.pw/privkey.pem"
	SSLCertificateChainFile "/etc/letsencrypt/live/api.arkumida.furtails.pw/fullchain.pem"
</VirtualHost>
