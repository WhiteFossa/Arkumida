# Arkumida
Furtails.pw next generation

# Build and run on Docker

# Infrastructure

Build image: $ docker build -f dockerfile-arkumida-infrastructure -t arkumida-infrastructure-postgres .
Run container: $ docker-compose -f docker-compose-arkumida-infrastructure.yml up -d

# Apply migrations

Build image: $ docker build -f dockerfile-migrator -t arkumida-migrator .
Run container: $ docker-compose -f docker-compose-arkumida-migrator.yml up

# WebAPI image

Build image: docker build -f dockerfile-webapi -t arkumida-webapi .
Run container: $ docker-compose -f docker-compose-arkumida-webapi.yml up -d

# Build frontend
Run (in Arkumida/vueapp directory): npm run build

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

# Configure redirecting to index.html (to allow client-side routing work)

(This section is for Apache, information for other servers is there: https://v3.router.vuejs.org/guide/essentials/history-mode.html)

Create .htaccess file with the next content in the root vhost directory:

<ifModule mod_rewrite.c>
    RewriteEngine On
    RewriteBase /
    RewriteRule ^index\.html$ - [L]
    RewriteCond %{REQUEST_FILENAME} !-f
    RewriteCond %{REQUEST_FILENAME} !-d
    RewriteRule . /index.html [L]
</ifModule>


# Notes
After running furtails-importer don't forget to replace [ascii=N] to [ascii=M], where M = N / 6, in text variants

# TODO
- Add images previews (for now full-sized images are displayed as previews)
