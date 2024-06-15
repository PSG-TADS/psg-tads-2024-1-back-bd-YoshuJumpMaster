document.addEventListener('DOMContentLoaded', function() {
    document.getElementById('loginForm').addEventListener('submit', function(event) {
        event.preventDefault();

        const username = document.getElementById('username').value;
        const password = document.getElementById('password').value;

        fetch('http://localhost:5236/api/auth/login', {
            method: 'POST',
            headers: {
                'Content-Type': 'application/json'
            },
            body: JSON.stringify({ username, password })
        })
        .then(response => response.json())
        .then(data => {
            if (data.token) {
                localStorage.setItem('token', data.token);
                localStorage.setItem('username', data.username);
                window.location.href = 'saldo.html';
            } else {
                document.getElementById('errorMessages').innerText = 'Falha no login, confira seus dados';
            }
        })
        .catch(error => {
            console.error('Error:', error);
            document.getElementById('errorMessages').innerText = 'Erro ao efeturar login =(';
        });
    });
});
