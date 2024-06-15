document.addEventListener('DOMContentLoaded', function() {
    document.getElementById('registerForm').addEventListener('submit', function(event) {
        event.preventDefault();

        const username = document.getElementById('username').value;
        const cpf = document.getElementById('cpf').value;

        fetch('http://localhost:5236/api/auth/register', {
            method: 'POST',
            headers: {
                'Content-Type': 'application/json'
            },
            body: JSON.stringify({ username, cpf })
        })
        .then(response => response.json())
        .then(data => {

            alert(data.Message);
            window.location.href = 'login.html';

        })
        .catch(error => {

            console.error('Error:', error);
            document.getElementById('errorMessages').innerText = 'Algo deu errado ao registrar seus dados =( ';
            
        });
    });
});
