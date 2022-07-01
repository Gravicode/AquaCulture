window.addEventListener('DOMContentLoaded', event => {
    // Simple-DataTables
    // https://github.com/fiduswriter/Simple-DataTables/wiki

   
});

function DemoDataTable(TableId) {
    const datatablesSimple = document.getElementById(TableId);
    if (datatablesSimple) {
        new simpleDatatables.DataTable(datatablesSimple);
    }
}