//Compose template string
String.prototype.compose = (function () {
    var re = /\{{(.+?)\}}/g;
    return function (o) {
        return this.replace(re, function (_, k) {
            return typeof o[k] != 'undefined' ? o[k] : '';
        });
    }
}());

$(document).ready(function () {

    $('#btnUpload').click(function () {
        if (HasUserChoosedFile()) InitializeUploadingSystem();
        else CleanFormData();
    });

    SwitchTitleIntoInput();
});

function SwitchTitleIntoInput() {
    var dataFile = $('#dataFile')[0];

    var label = dataFile.nextElementSibling;
    var labelVal = label.innerHTML;

    // write filename into label if was selected otherwise reset the label
	dataFile.addEventListener('change', function (e) {
        var fileName = e.target.value.split('\\').pop();

        if (fileName)
            label.querySelector('span').innerHTML = fileName;
        else
            label.innerHTML = labelVal;
    });

    // Firefox bug fix
    dataFile.addEventListener('focus', function () { dataFile.classList.add('has-focus'); });
    dataFile.addEventListener('blur', function () { dataFile.classList.remove('has-focus'); });
}

function CleanFormData() {
    var resultData = $("#result_data");
    var label = $('#btnUpload')[0].nextElementSibling;

    resultData.html('');
    label.innerHTML = '';
}

function HasUserChoosedFile() {
    return $('#dataFile') != null && $('#dataFile').length > 0 && $('#dataFile')[0].files != null && $('#dataFile')[0].files.length > 0;
}

function InitializeUploadingSystem() {
    $.ajax({
        type: "POST",
        url: UrlSettings.InitializeUploadingSystem,
        success: function (data) {
            var dataFile = $('#dataFile')[0];
            var label = $('#btnUpload')[0].nextElementSibling;
            if (typeof data.Message !== "undefined") label.innerHTML = data.Message;
            else label.innerHTML = "Start uploading file...";
            UploadFile(dataFile.files);
        },
        error: function (jqXHR) {
            var label = $('#btnUpload')[0].nextElementSibling;
            if (typeof jqXHR.statusText !== "undefined" && typeof jqXHR.statusText != '') label.innerHTML = jqXHR.statusText;
            else label.innerHTML = "The uploaded folder has not be cleaned!";
        }
    });
}

function UploadFile(TargetFile) {
    // create array to store the buffer chunks
    var FileChunk = [];

    // the file object itself that we will work with
    var file = TargetFile[0];

    // set up other initial vars
    var BufferChunkSize = 1 * (1024 * 1024);
    var FileStreamPos = 0;

    // set the initial chunk length
    var EndPos = BufferChunkSize;
    var Size = file.size;

    // add to the FileChunk array until we get to the end of the file
    while (FileStreamPos < Size) {
        // "slice" the file from the starting position/offset, to  the required length
        FileChunk.push(file.slice(FileStreamPos, EndPos));
        FileStreamPos = EndPos; // jump by the amount read
        EndPos = FileStreamPos + BufferChunkSize; // set next chunk length
    }

    // get total number of "files" we will be sending
    var TotalParts = FileChunk.length;
    var PartCount = 0;

    // loop through, pulling the first item from the array each time and sending it
    while (chunk = FileChunk.shift()) {
        PartCount++;
        // file name convention
        var FilePartName = file.name + ".part_" + PartCount + "." + TotalParts;
        // send the chunk file
        UploadFileChunk(chunk, FilePartName);
    }
}

function UploadFileChunk(Chunk, FileName) {
    var FD = new FormData();
    FD.append('file', Chunk, FileName);
    $.ajax({
        type: "POST",
        url: UrlSettings.MergeFilesUploadedIntoSingleFile,
        contentType: false,
        processData: false,
        data: FD,
        success: function (data) {
            var label = $('#btnUpload')[0].nextElementSibling;
            if (typeof data.BaseFileName !== "undefined" && typeof data.Chunk !== "undefined" && typeof data.MaxChunks !== "undefined") {
                if (data.BaseFileName != "") {
                    if (typeof data.Message !== "undefined") label.innerHTML = data.Message + " Start parsing...";
                    else label.innerHTML = "File Uploaded! Start parsing...";
                    CallGetDataIntoJSON(data.BaseFileName);
                } else { // data.Chunk <= data.MaxChunks
                    if (typeof data.Message !== "undefined") label.innerHTML = data.Message;
                    else label.innerHTML = "Uploaded chunk n. " + data.Chunk + "/" + data.MaxChunks + "!";
                }
            } else {
                label.innerHTML = "Unsupported results!";
                if (typeof data.Message !== "undefined") label.innerHTML += " " + data.Message;
            }
        },
        error: function (jqXHR) {
            var label = $('#btnUpload')[0].nextElementSibling;
            if (jqXHR.status === 401) {
                label.innerHTML = jqXHR.statusText;
            } else {
                label.innerHTML = "Unsupported exception during uploading file!";
            }            
        }
    });
}

function CallGetDataIntoJSON(messageText) {
    $.ajax({
        type: "POST",
        url: UrlSettings.GetDataIntoJSON,
        data: { baseFileName: messageText },
        success: function (data) {
            var label = $('#btnUpload')[0].nextElementSibling;
            if (typeof data.Message !== "undefined") label.innerHTML = data.Message;
            else label.innerHTML = "File uploaded and parsed!";
            PrintDataIntoTable(data);
        },
        error: function (jqXHR) {
            var label = $('#btnUpload')[0].nextElementSibling;
            if (jqXHR.status === 401) {
                label.innerHTML = jqXHR.statusText;
            } else {
                label.innerHTML = "Unsupported exception during parsing file!";
            }
        }
    });
}

function PrintDataIntoTable(data) {
    var resultData = $("#result_data");
    resultData.html('');

    var table = $("<table></table>");
    table.addClass("table_overflow");

    var headerRow = '<tr>' +
                            '<th class="th_1">Client IP</th>' +
                            '<th class="th_2">FQDN</th>' +
                            '<th class="th_3">N. Calls</th>' +
                      '</tr>';

    var thead = $("<thead></thead>");
    thead.append(headerRow);

    table.append(thead);

    var tbody = $("<tbody></tbody>");

    var row = '<tr>' +
                    '<td>{{clientIp}}</td>' +
                    '<td>{{fqdns}}</td>' +
                    '<td>{{nCalls}}</td>' +
              '</tr>';

    $.each(JSON.parse(data.JsonData), function (i, client) {
        var fqdns = '';

        $.each(client.FQDNs, function (j, fqdn) {
            fqdns += fqdn + '; ';
        });

        tbody.append(row.compose({
            'clientIp': client.ClientIp,
            'fqdns': fqdns,
            'nCalls': client.NCalls
        }));
    });

    table.append(tbody);

    resultData.append(table);
}