document.addEventListener('DOMContentLoaded', function() {
    document.getElementById('recargaForm').addEventListener('submit', function(event) {
        event.preventDefault();

        const token = localStorage.getItem('token');
        const username = localStorage.getItem('username');
        const valor = document.getElementById('valor').value;

        fetch('http://localhost:5236/api/recarga', {
            method: 'POST',
            headers: {
                'Content-Type': 'application/json',
                'Authorization': `Bearer ${token}`,
                'Username': username
            },
            body: JSON.stringify({ valor: parseFloat(valor) })
        })
        .then(response => response.json())
        .then(data => {

            alert(data.Message);
            window.location.reload();
            
        })
        .catch(error => {

            console.error('erro ', error);
            document.getElementById('errorMessages').innerText = 'Erro ao efetuar sua recarga =(';

        });
    });
});
