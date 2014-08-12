

function tooltipify($labels){
    $labels.each(function () {
        $(this).attr('title', $(this).text());
    });
}

function openAngularException(response)
{
    
    $.fancybox.open([
    {
        href: '#angular-exception', scrolling: 'no'
    }]);

    if (response)
    {
        $('#angular-exception #statusCode').text(response.status);
        $('#angular-exception #statusText').text(response.statusText);

        if (response.data)
        {
            $('#angular-exception #exType').text(response.data.exceptionType);
            $('#angular-exception #exMessage').text(response.data.exceptionMessage);
            $('#angular-exception #message').text(response.data.message);
            $('#angular-exception #stacktrace').val(response.data.stackTrace);
        }
    }
}