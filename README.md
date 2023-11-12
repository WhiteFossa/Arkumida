# Arkumida
Furtails.pw next generation

# Build and run on Docker

# Create network (to avoid problems with directory prefixes)

docker network create -d bridge arkumida-net

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

    ProxyPass / http://127.0.0.1:5220/ connectiontimeout=30 timeout=300
    ProxyPassReverse / http://127.0.0.1:5220/
    ProxyRequests Off

	SSLEngine on
	SSLCertificateFile "/etc/letsencrypt/live/api.arkumida.furtails.pw/cert.pem"
	SSLCertificateKeyFile "/etc/letsencrypt/live/api.arkumida.furtails.pw/privkey.pem"
	SSLCertificateChainFile "/etc/letsencrypt/live/api.arkumida.furtails.pw/fullchain.pem"
</VirtualHost>

# Configure static content virtual host
Example for Apache:

##############################################################################
#                           arkumida.furtails.pw                              #
##############################################################################
<VirtualHost *:80>
    ServerAdmin whitefossa@protonmail.com
    DocumentRoot "/webroot/vhosts/arkumida.furtails.pw/htdocs"
    ServerName arkumida.furtails.pw
    ErrorLog "/webroot/vhosts/arkumida.furtails.pw/logs/error.log"
    CustomLog "/webroot/vhosts/arkumida.furtails.pw/logs/access.log" combined

    Redirect permanent / https://arkumida.furtails.pw/
</VirtualHost>

# Secure
<VirtualHost *:443>
    ServerAdmin whitefossa@protonmail.com
    DocumentRoot "/webroot/vhosts/arkumida.furtails.pw/htdocs"
    ServerName arkumida.furtails.pw
    ErrorLog "/webroot/vhosts/arkumida.furtails.pw/logs/error.log"
    CustomLog "/webroot/vhosts/arkumida.furtails.pw/logs/access.log" combined

    # Static content compression
    AddOutputFilterByType BROTLI_COMPRESS text/html text/css text/javascript application/javascript

	SSLEngine on
	SSLCertificateFile "/etc/letsencrypt/live/arkumida.furtails.pw/cert.pem"
	SSLCertificateKeyFile "/etc/letsencrypt/live/arkumida.furtails.pw/privkey.pem"
	SSLCertificateChainFile "/etc/letsencrypt/live/arkumida.furtails.pw/fullchain.pem"

	<FilesMatch "\.(cgi|shtml|phtml|php)$">
		SSLOptions +StdEnvVars
	</FilesMatch>

	<Directory "/webroot/vhosts/arkumida.furtails.pw/htdocs">
		SSLOptions +StdEnvVars
		Options SymLinksIfOwnerMatch
		AllowOverride All
		Require all granted
	</Directory>
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

# Content compression

Do not forget to add this to static (arkumida.furtails.pw) virtual host:

# Static content compression
AddOutputFilterByType BROTLI_COMPRESS text/html text/css text/javascript application/javascript

But think first about BREACH and CRIME attacks!

# Notes
After running furtails-importer don't forget to replace [ascii=N] to [ascii=M], where M = N / 6, in text variants

# TODO
- Add images previews (for now full-sized images are displayed as previews)
- Fix incorrect search for M and F tags
