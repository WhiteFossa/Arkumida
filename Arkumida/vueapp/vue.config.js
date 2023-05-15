module.exports = {
    devServer:
    {
        proxy:
        {
            '^/api':
            {
                target: 'http://localhost:5220'
            }
        },
        port: 5002
    }
}