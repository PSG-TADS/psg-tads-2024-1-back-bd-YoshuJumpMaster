document.addEventListener('DOMContentLoaded', function() {
    
    const token = localStorage.getItem('token');
    const username = localStorage.getItem('username');

    fetch('http://localhost:5236/api/saldo', {
        method: 'GET',
        headers: {
            'Authorization': `Bearer ${token}`,
            'Username': username
        }
    })
    .then(response => response.json())
    .then(data => {

        document.getElementById('saldo').innerText = data.saldo;

    })
    .catch(error => {

        console.error('erro  ', error);
        document.getElementById('errorMessages').innerText = 'erro no fetch de saldo';

    });
});
