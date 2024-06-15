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
        .then(response => {
            if (!response.ok) {
                return response.json().then(err => { throw new Error(err.message || 'Recarga falhou'); });
            }
            return response.json();
        })
        .then(data => {
            alert(data.Message || 'Recarga realizada com sucesso!');
            window.location.reload();
        })
        .catch(error => {
            console.error('Erro:', error);
            document.getElementById('errorMessages').innerText = 'Erro ao efetuar sua recarga =(';
        });
    });
});
