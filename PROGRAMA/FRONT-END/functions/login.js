let url = 'https://localhost:44358/api/Usuario/Login';
const metodo = 'Post';

document.getElementById('btnSave').addEventListener('click', (event) => {
    event.preventDefault(); 
    let credenciales = {
        nombre: document.getElementById('user').value.trim(),
        pass: document.getElementById('pass').value.trim(),
    };
    fetch(url, {
        method: metodo,
        headers: { 'Content-type': 'application/json' },
        body: JSON.stringify(credenciales)
    }).then(response => response.json())
        .then(result => {
            if (result.success) {
                Swal.fire(result.message, 'Éxito', 'success');
                alert('Credenciales Correctas');
                window.location.href = 'menu.html';
            } else {
                Swal.fire(result.message, 'Error', 'warning');
            }
        });
});
