# Arkumida
Furtails.pw next generation

# Build and run on Docker

# Webapi image

docker build -f dockerfile-webapi -t arkumida-webapi .

# Configure reverse proxy for API server
Configure reverse proxy in a such way (example for Apache):

##############################################################################
#                           api.arkumida.fossa.life                         #
##############################################################################
<VirtualHost *:80>
    ServerName api.arkumida.fossa.life
    ServerAdmin whitefossa@protonmail.com
    ErrorLog "/webroot/vhosts/api.arkumida.fossa.life/logs/error.log"
    CustomLog "/webroot/vhosts/api.arkumida.fossa.life/logs/access.log" combined

    ProxyPass / http://127.0.0.1:5220/
    ProxyPassReverse / http://127.0.0.1:5220/
    ProxyRequests Off
</VirtualHost>

<VirtualHost *:443>
    ServerName api.arkumida.fossa.life
    ServerAdmin whitefossa@protonmail.com
    ErrorLog "/webroot/vhosts/api.arkumida.fossa.life/logs/error.log"
    CustomLog "/webroot/vhosts/api.arkumida.fossa.life/logs/access.log" combined

    ProxyPass / http://127.0.0.1:5220/
    ProxyPassReverse / http://127.0.0.1:5220/
    ProxyRequests Off

	SSLEngine on
	SSLCertificateFile "/etc/letsencrypt/live/api.arkumida.fossa.life/cert.pem"
	SSLCertificateKeyFile "/etc/letsencrypt/live/api.arkumida.fossa.life/privkey.pem"
	SSLCertificateChainFile "/etc/letsencrypt/live/api.arkumida.fossa.life/fullchain.pem"
</VirtualHost>
