    function determineHierarchy(value) {
        var file = $('#fileUpload')[0].files[0];
        var fd = new FormData();
        fd.append('theFile', file);
        $.ajax({
            type: 'post',
            url: '?handler=AnalyseFile',
            processData: false,
            contentType: false,
            data: fd,
            success: function (response) {
                $('#output').empty();
                var toAppend = "";
                if (response.includes('Error encountered')) {
                    toAppend += "<h3>" + response + "</h3>"
                } else {
                    toAppend += "<ul>";
                    toAppend += "";
                    for (var i = 0; i < response.length; i++) {

                        toAppend += "<li>" + response[i].frequency + " " + response[i].word + "</li>";
                    }
                    toAppend += "";
                    toAppend += "</ul>";
                }
                $('#output').append(toAppend);
            }
        })
    }