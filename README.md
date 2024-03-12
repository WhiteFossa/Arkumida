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

Apache example:

```
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
```

Nginx example:

```
# api.arkumida.furtails.pw

# HTTP - api.arkumida.furtails.pw -> HTTPS
server
{
    listen 80;
    server_name api.arkumida.furtails.pw;

    # Redirect to HTTPS
    if ($host = api.arkumida.furtails.pw)
    {
        return 301 https://$host$request_uri;
    }

    return 404;
}

# HTTPS - api.arkumida.furtails.pw
server
{
    listen 443 ssl http2;
    server_name api.arkumida.furtails.pw;

    # Proxying to Arkumida backend
    location /
    {
        proxy_pass http://127.0.0.1:5220;
        proxy_connect_timeout 300;
        proxy_read_timeout 300;
        proxy_send_timeout 300;

        client_max_body_size 512M;
    }

    # SSL
    ssl_certificate /etc/letsencrypt/live/api.arkumida.furtails.pw/fullchain.pem;
    ssl_certificate_key /etc/letsencrypt/live/api.arkumida.furtails.pw/privkey.pem;
    include /etc/letsencrypt/options-ssl-nginx.conf;
    ssl_dhparam /etc/letsencrypt/ssl-dhparams.pem;
}
```


# Configure static content virtual host

Apache example:

```
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
```

Nginx example:

```
# arkumida.furtails.pw

# HTTP - arkumida.furtails.pw -> HTTPS
server
{
    listen 80;
    server_name arkumida.furtails.pw;

    # Redirects to HTTPS
    if ($host = arkumida.furtails.pw)
    {
        return 301 https://$host$request_uri;
    }

    return 404;
}

# HTTPS - arkumida.furtails.pw
server
{
    listen 443 ssl http2;
    server_name arkumida.furtails.pw;

    # Webroot
    root /webroot/arkumida.furtails.pw/htdocs;

    # Logs
    access_log /webroot/arkumida.furtails.pw/logs/access.log;
    error_log /webroot/arkumida.furtails.pw/logs/error.log error;

    # Indexes
    index index.html;

    # SSL
    ssl_certificate /etc/letsencrypt/live/arkumida.furtails.pw/fullchain.pem;
    ssl_certificate_key /etc/letsencrypt/live/arkumida.furtails.pw/privkey.pem;
    include /etc/letsencrypt/options-ssl-nginx.conf;
    ssl_dhparam /etc/letsencrypt/ssl-dhparams.pem;

    # VueJS rewrites (for History Mode)
    location /
    {
        try_files $uri $uri/ /index.html;
    }

	# Huge uploads
	client_max_body_size 1024M;

	# GZIP
    gzip on;
    gzip_comp_level 9;
    gzip_proxied any;
    gzip_vary on;

    gzip_types
        application/atom+xml
        application/geo+json
        application/javascript
        application/x-javascript
        application/json
        application/ld+json
        application/manifest+json
        application/rdf+xml
        application/rss+xml
        application/xhtml+xml
        application/xml
        font/eot
        font/otf
        font/ttf
        image/svg+xml
        text/css
        text/javascript
        text/plain
        text/xml

    server_names_hash_bucket_size 128;
}

```

# (Apache only!) Configure redirecting to index.html (to allow client-side routing work)

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

# (Apache only!) Content compression
Do not forget to add this to static (arkumida.furtails.pw) virtual host:

```
# Static content compression
AddOutputFilterByType BROTLI_COMPRESS text/html text/css text/javascript application/javascript
```

But think first about BREACH and CRIME attacks!

# Notes
After running furtails-importer don't forget to replace [ascii=N] to [ascii=M], where M = N / 6, in text variants

# Resetting database (before importing data again)

1) Stop arkumida-webapi container
2) Drop Arkumida (with capital A) database
3) Open OpenSearch control port by adding

ports:
      - 0.0.0.0:9200:9200 # Do not need it in Prod

to docker-compose-arkumida-infrastructure.yml and restarting infrastructure container

4) Clear opensearch data:
curl --request DELETE --url http://127.0.0.1:9200/creatures
curl --request DELETE --url http://127.0.0.1:9200/tags
curl --request DELETE --url http://127.0.0.1:9200/texts

5) Close OpenSearch control port and restart Infrastructure container again
6) Run migrations
7) Start arkumida-webapi container
8) Run furtails-importer

# TODO
- Add images previews (for now full-sized images are displayed as previews)
- Add targeted links for authors/publishers/translators in texts
