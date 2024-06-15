document.addEventListener('DOMContentLoaded', function() {
    const token = localStorage.getItem('token');
    const username = localStorage.getItem('username');
    const reservaSelect = document.getElementById('reserva');
    const valorInput = document.getElementById('valor');
    const errorMessages = document.getElementById('errorMessages');

    fetch(`http://localhost:5236/api/reservas/cliente`, {
        method: 'GET',
        headers: {
            'Authorization': `Bearer ${token}`,
            'Username': username
        }
    })
    .then(response => {
        if (!response.ok) {
            return response.json().then(err => { throw new Error(err.message || 'Failed to fetch reservas') });
        }
        return response.json();
    })
    .then(data => {
        data.forEach(reserva => {
            const option = document.createElement('option');
            option.value = reserva.reservaId;
            option.text = `${reserva.reservaId} - ${reserva.veiculoModelo}`;
            reservaSelect.appendChild(option);
        });

        reservaSelect.addEventListener('change', function() {
            const selectedReserva = data.find(reserva => reserva.reservaId == reservaSelect.value);
            if (selectedReserva) {
                valorInput.value = selectedReserva.valorTotal;
            } else {
                valorInput.value = '';
            }
        });
    })
    .catch(error => {
        console.error('Error:', error);
        errorMessages.innerText = `Error: ${error.message}`;
    });

    document.getElementById('pagar').addEventListener('click', function() {
        const selectedReservaId = reservaSelect.value;
        const valor = valorInput.value;

        if (!selectedReservaId || !valor) {
            errorMessages.innerText = 'Por favor, selecione uma reserva válida.';
            return;
        }

        fetch(`http://localhost:5236/api/pagamento`, {
            method: 'POST',
            headers: {
                'Content-Type': 'application/json',
                'Authorization': `Bearer ${token}`,
                'Username': username
            },
            body: JSON.stringify({ ReservaId: selectedReservaId, Valor: parseFloat(valor), Data: new Date() })
        })
        .then(response => {

            if (!response.ok) {

                return response.json().then(err => { throw new Error(err.message || 'Pagamento falhou =( ') });

            }
            return response.json();
        })
        .then(data => {

            alert(data.message);
            window.location.reload();

        })
        .catch(error => {

            console.error('erro', error);
            errorMessages.innerText = `erro  ${error.message}`;
            
        });
    });
});
