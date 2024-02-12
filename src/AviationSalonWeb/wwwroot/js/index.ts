// wwwroot/js/index.ts
$(document).ready(function () {
    // Handle "View Details" button click
    $(document).on('click', '.view-details-btn', function () {
        var aircraftId = $(this).data('id');
        loadDetails(aircraftId);
    });

    // Handle "Equip Aircraft" button click
    $(document).on('click', '.equip-aircraft-btn', function () {
        var aircraftId = $(this).data('id');
        loadEquipPage(aircraftId);
    });
});

function loadDetails(aircraftId: number): void {
    // Make an Ajax request to load details page for the selected aircraft
    $.ajax({
        url: `/Aircraft/Details/${aircraftId}`,
        method: 'GET',
        success: function (data) {
            // Replace the current page content with details page content
            $('body').html(data);
        },
        error: function () {
            alert('Error loading details.');
        }
    });
}

function loadEquipPage(aircraftId: number): void {
    // Make an Ajax request to load equip page for the selected aircraft
    $.ajax({
        url: `/Aircraft/Equip/${aircraftId}`,
        method: 'GET',
        success: function (data) {
            // Replace the current page content with equip page content
            $('body').html(data);
        },
        error: function () {
            alert('Error loading equip page.');
        }
    });
}
