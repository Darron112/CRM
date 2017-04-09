var Users = []
//fetch categories from database
function LoadUser(element) {
    if (Users.length == 0) {
        //ajax function for fetch data
        $.ajax({
            type: "GET",
            url: '/request/getUser',
            data: { 'UserID': $(element).val() },
            success: function (data) {
                Users = data;
                //render catagory
                renderUser(element);
            },
            error: function (error) {
                console.log(error);
            }
        })
    }
    //else {
    //    //render catagory to the element
    //    renderUser(element);
    //}
}

function renderUser(element) {
    var $ele = $(element);
    $ele.empty();
    $ele.append($('<option/>').val('0').text('Select'));
    $.each(Users, function (i, val) {
        $ele.append($('<option/>').val(val.UserID).text(val.UserName));
    })
}

var Status = []
//fetch categories from database
function LoadStatus(element) {
    if (Status.length == 0) {
        //ajax function for fetch data
        $.ajax({
            type: "GET",
            url: '/request/getStatus',
            data: { 'StatusID': $(element).val() },
            success: function (data) {
                Status = data;
                //render catagory
                renderStatus(element);
            },
            error: function (error) {
                console.log(error);
            }
        })
    }
    //else {
    //    //render catagory to the element
    //    renderStatus(element);
    //}
}

function renderStatus(element) {
    var $ele = $(element);
    $ele.empty();
    $ele.append($('<option/>').val('0').text('Select'));
    $.each(Status, function (i, val) {
        $ele.append($('<option/>').val(val.StatusID).text(val.StatusName));
    })
}

var Client = []
//fetch categories from database
function LoadClient(element) {
    if (Client.length == 0) {
        //ajax function for fetch data
        $.ajax({
            type: "GET",
            url: '/request/getClient',
            data: { 'ClientID': $(element).val() },
            success: function (data) {
                Client = data;
                //render catagory
                renderClient(element);
            },
            error: function (error) {
                console.log(error);
            }
        })
    }
    //else {
    //    //render catagory to the element
    //    renderStatus(element);
    //}
}

function renderClient(element) {
    var $ele = $(element);
    $ele.empty();
    $ele.append($('<option/>').val('0').text('Select'));
    $.each(Client, function (i, val) {
        $ele.append($('<option/>').val(val.ClientID).text(val.CompanyName));
    })
}

var Type = []
//fetch categories from database
function LoadType(element) {
    if (Type.length == 0) {
        //ajax function for fetch data
        $.ajax({
            type: "GET",
            url: '/request/getType',
            data: { 'TypeID': $(element).val() },
            success: function (data) {
                Type = data;
                //render catagory
                renderType(element);
            },
            error: function (error) {
                console.log(error);
            }
        })
    }
    //else {
    //    //render catagory to the element
    //    renderStatus(element);
    //}
}

function renderType(element) {
    var $ele = $(element);
    $ele.empty();
    $ele.append($('<option/>').val('0').text('Select'));
    $.each(Type, function (i, val) {
        $ele.append($('<option/>').val(val.TypeID).text(val.TypeName));
    })
}

$(document).ready(function () {
    //Add button click event
    $('#add').click(function () {
        //validation and add order items
        var isAllValid = true;
        if ($('#user').val() == "0") {
            isAllValid = false;
            $('#user').siblings('span.error').css('visibility', 'visible');
        }
        else {
            $('#user').siblings('span.error').css('visibility', 'hidden');
        }

        if ($('#status').val() == "0") {
            isAllValid = false;
            $('#status').siblings('span.error').css('visibility', 'visible');
        }
        else {
            $('#status').siblings('span.error').css('visibility', 'hidden');
        }

        if (!($('#stageNumber').val().trim() != '' && (parseInt($('#stageNumber').val()) || 0))) {
            isAllValid = false;
            $('#stageNumber').siblings('span.error').css('visibility', 'visible');
        }
        else {
            $('#stageNumber').siblings('span.error').css('visibility', 'hidden');
        }

        if (!($('#stageTime').val().trim() != '' && !isNaN($('#stageTime').val().trim()))) {
            isAllValid = false;
            $('#stageTime').siblings('span.error').css('visibility', 'visible');
        }
        else {
            $('#stageTime').siblings('span.error').css('visibility', 'hidden');
        }        
        
        if (isAllValid) {
            var $newRow = $('#mainrow').clone().removeAttr('id');
            $('.user', $newRow).val($('#user').val());
            $('.status', $newRow).val($('#status').val());
            //$('.client', $newRow).val($('#client').val());
            //$('.type', $newRow).val($('#type').val());

            //Replace add button with remove button
            $('#add', $newRow).addClass('remove').val('Remove').removeClass('btn-success').addClass('btn-danger');

            //remove id attribute from new clone row
            $('#user,#status,#stageNumber,#stageTime,#add', $newRow).removeAttr('id');
            $('span.error', $newRow).remove();
            //append clone row
            $('#requestdetailsItems').append($newRow);

            //clear select data
            $('#user,#status').val('0');
            $('#stageNumber,#stageTime').val('');
            $('#requestItemError').empty();
        }

    })

    //remove button click event
    $('#requestdetailsItems').on('click', '.remove', function () {
        $(this).parents('tr').remove();
    });

    $('#submit').click(function () {
        var isAllValid = true;

        //if (isAllValid) {
        //    var $newRow = $('#secrow').removeAttr('id');
        //    $('.client', $newRow).val($('#client').val());
        //    $('.type', $newRow).val($('#type').val());

        //    //remove id attribute from new clone row
        //    //$('#submit, #client, #type', $newRow).removeAttr('id');
        //    //$('span.error', $newRow).remove();
        //    //append clone row
        //    //$('#requestdetailsItems').append($newRow);

        //    //clear select data
        //    //$('#client, #type').val('0');
        //    //$('#requestItemError').empty();
        //}

        //validate order items
        $('#requestItemError').text('');
        var list = [];
        var errorItemCount = 0;
        $('#requestdetailsItems tbody tr').each(function (index, ele) {
            if (
                $('select.status', this).val() == "0" ||
                $('select.user', this).val() == "0" ||
                //$('select.client', this).val() == "0" ||
                //$('select.type', this).val() == "0" ||
                (parseInt($('.stageNumber', this).val()) || 0) == 0 ||
                $('.stageTime', this).val() == "" ||
                isNaN($('.stageTime', this).val())
                ) {
                errorItemCount++;
                $(this).addClass('error');
            } else {
                var requestItem = {
                    StatusID: $('select.status', this).val(),
                    UserID: $('select.user', this).val(),
                    StageNumber: parseInt($('.stageNumber', this).val()),
                    StageTime: parseFloat($('.stageTime', this).val())
                }
                list.push(requestItem);
            }
        })

        if (errorItemCount > 0) {
            $('#requestItemError').text(errorItemCount + " Nieprawidlowy wpis.");
            isAllValid = false;
        }

        if (list.length == 0) {
            $('#requestItemError').text('Przynajmniej 1 etap jest wymagany.');
            isAllValid = false;
        }

        if ($('#title').val().trim() == '') {
            $('#title').siblings('span.error').css('visibility', 'visible');
            isAllValid = false;
        }
        else {
            $('#title').siblings('span.error').css('visibility', 'hidden');
        }

        if ($('#requestDate').val().trim() == '') {
            $('#requestDate').siblings('span.error').css('visibility', 'visible');
            isAllValid = false;
        }
        else {
            $('#requestDate').siblings('span.error').css('visibility', 'hidden');
        }

        if ($('#client').val() == "0") {
            isAllValid = false;
            $('#client').siblings('span.error').css('visibility', 'visible');
        }
        else {
            $('#client').siblings('span.error').css('visibility', 'hidden');
        }

        if ($('#type').val() == "0") {
            isAllValid = false;
            $('#type').siblings('span.error').css('visibility', 'visible');
        }
        else {
            $('#type').siblings('span.error').css('visibility', 'hidden');
        }

        if (isAllValid) {
            var data = {
                //RequestDetailsID: $('requestDetailsID').val().trim(),
                Title: $('#title').val().trim(),
                RequestDateString: $('#requestDate').val().trim(),
                Description: $('#description').val().trim(),
                ClientID: $('#client').val(),
                TypeID: $('#type').val(),
                RequestDetails: list
            }

            $(this).val('Prosze czekac...');

            $.ajax({
                type: 'POST',
                url: '/request/save',
                data: JSON.stringify(data),
                contentType: 'application/json',
                success: function (data) {
                    if (data.status) {
                        alert('Zapisane pomyslnie');
                        //here we will clear the form
                        list = [];
                        $('#title,#requestDate,#description,#client,#type').val('');
                        $('#requestdetailsItems').empty();
                    }
                    else {
                        alert('Blad');
                    }
                    $('#submit').text('Save');
                },
                error: function (error) {
                    console.log(error);
                    $('#submit').text('Save');
                }
            });
        }

    });

});

LoadUser($('#user'));
LoadStatus($('#status'));
LoadClient($('#client'));
LoadType($('#type'));
