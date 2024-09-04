$(function () {
    goSearch();


    $("#btn_search").click(function () {
        goSearch();
    });

    $("#stx").keyup(function (e) {
        if (e.keyCode == 13) {
            goSearch();
        }
    });


});


function goView(pageID) {
    location.href = "/Recruitment/view?id=" + pageID;
}

function goPage(pageIndex) {
    _fnSearchData(pageIndex);
}

function goSearch() {
    _fnSearchData(1);
}

function _fnSearchData(pageIndex) {

    var opt = $("#sf1 option:selected").val();
    var txtVal = $("#stx").val();

    var rtnJson;
    var rtnVal;
    var callObj = new Object();

    callObj.Option = opt;
    callObj.SearchText = txtVal;
    callObj.Page = pageIndex;

    $.ajax({
        type: "POST",
        url: "/Recruitment/CallAjax",
        async: false,
        dataType: "json",
        data: callObj,
        success: function (result) {
            rtnVal = result;    //JSON.stringify(result);
        }, error: function (xhr) {
            console.log("시스템 사정으로 요청하신 작업을 처리할 수 없습니다.");
            console.log(xhr);
            return;
        }
    });

    $(".board_list > table").remove();

    var innerText = "";
    var innerPage = "";
    var totPageCnt;
    var maxPageCnt = 0;
    var nPage = 1;
    var language = document.documentElement.lang;
    
    if (rtnVal == undefined) {
        innerText += "<table class='notice-table'> ";
        innerText += "	<colgroup> ";
        innerText += "		<col /> ";
        innerText += "		<col style='width:8em' /> ";
        innerText += "		<col style='width:10em' /> ";
        innerText += "	</colgroup> ";
        if (language === 'ko') {
            innerText += "	<thead> ";
            innerText += "		<tr> ";
            innerText += "			<th scope='col'>제목</th> ";
            innerText += "			<th scope='col'>작성자</th> ";
            innerText += "			<th scope='col'>등록일</th> ";
            innerText += "		</tr> ";
            innerText += "	</thead> ";
        } else {
            innerText += "	<thead> ";
            innerText += "		<tr> ";
            innerText += "			<th scope='col'>Subjects</th> ";
            innerText += "			<th scope='col'>Preparers</th> ";
            innerText += "			<th scope='col'>Registration Date</th> ";
            innerText += "		</tr> ";
            innerText += "	</thead> ";
        }
        innerText += "<tbody> ";
        if (language === 'ko') {
            innerText += "<tr> ";
            innerText += "<td colspan=\"3\">등록된 모집공고가 없습니다</td>";
            innerText += "</tr> ";
        } else {
            innerText += "<tr> ";
            innerText += "<td colspan=\"3\">There is no registered recruitment announcement.</td>";
            innerText += "</tr> ";
        }
        innerText += "</tbody> ";
        innerText += "</table> ";
    } else {
        innerText += "<table class='notice-table'> ";
        innerText += "	<colgroup> ";
        innerText += "		<col /> ";
        innerText += "		<col style='width:8em' /> ";
        innerText += "		<col style='width:10em' /> ";
        innerText += "	</colgroup> ";
        if (language === 'ko') {
            innerText += "	<thead> ";
            innerText += "		<tr> ";
            innerText += "			<th scope='col'>제목</th> ";
            innerText += "			<th scope='col'>작성자</th> ";
            innerText += "			<th scope='col'>등록일</th> ";
            innerText += "		</tr> ";
            innerText += "	</thead> ";
        } else {
            innerText += "	<thead> ";
            innerText += "		<tr> ";
            innerText += "			<th scope='col'>Subjects</th> ";
            innerText += "			<th scope='col'>Preparers</th> ";
            innerText += "			<th scope='col'>Registration Date</th> ";
            innerText += "		</tr> ";
            innerText += "	</thead> ";
        }
        innerText += "	<tbody> ";

        $(rtnVal).each(function (i) {
            totPageCnt = rtnVal[i].TOTCNT;
            maxPageCnt = maxPageCnt + 1;
            nPage = rtnVal[i].PAGE;

            if (rtnVal[i].NOTICE_YN == "y") {
                innerText += "<tr> ";
                innerText += "	<td onclick='goView(" + rtnVal[i].NOTICE_ID + ")'><a href='#'>" + rtnVal[i].TITLE + "</a></td> ";
                innerText += "	<td>" + rtnVal[i].WRITER + "</td> ";
                innerText += "	<td>" + rtnVal[i].REGDT.substring(0, 10) + "</td> ";
                innerText += "</tr> ";
            } else {
                innerText += "<tr> ";
                innerText += "	<td onclick='goView(" + rtnVal[i].NOTICE_ID + ")'><a href='#'>" + rtnVal[i].TITLE + "</a></td> ";
                innerText += "	<td>" + rtnVal[i].WRITER + "</td> ";
                innerText += "	<td>" + rtnVal[i].REGDT.substring(0, 10) + "</td> ";
                innerText += "</tr> ";
            }
        });
        innerText += "		</tbody> ";
        innerText += "	</table> ";

        fnPaging(totPageCnt, 10, 5, pageIndex);
    }



    $(".board_list").append(innerText);
}

//totalData = 총 데이터 count
//dataPerPage = 한페이지에 나타낼 데이터 수
// pageCount = 한화면에 나타낼 페이지 수
//currentPage = 선택한 페이지 
function fnPaging(totalData, dataPerPage, pageCount, currentPage) {

    var totalPage = Math.ceil(totalData / dataPerPage);    // 총 페이지 수
    var pageGroup = Math.ceil(currentPage / pageCount);    // 페이지 그룹
    if (pageCount > totalPage) pageCount = totalPage;
    var last = pageGroup * pageCount;    // 화면에 보여질 마지막 페이지 번호
    if (last > totalPage) last = totalPage;
    var first = last - (pageCount - 1);    // 화면에 보여질 첫번째 페이지 번호
    var next = last + 1;
    var prev = first - 1;

    $(".paging-area .paging_in").remove();

    var prevPage;
    var nextPage;
    if (currentPage - 1 < 1) { prevPage = 1; } else { prevPage = currentPage - 1; }
    if (last < totalPage) { nextPage = currentPage + 1; } else { nextPage = last; }

    var html = "";

    html += "<div class='paging_in'> ";
    html += "	<a href='#' onclick='goPage(1)' class='prev-first'><span class='blind'>처음페이지로 가기</span></a> ";
    html += "	<a href='#' onclick='goPage(" + prevPage + ")' class='prev'><span class='blind'>이전페이지로 가기</span></a> ";
    html += "	<span class='number'> ";

    for (var i = first; i <= last; i++) {

        if (i == currentPage) {
            html += "		<a href='#' onclick='goPage(" + i + ")' class='active'>" + i + "</a> ";
        } else {
            html += "		<a href='#' onclick='goPage(" + i + ")'>" + i + "</a> ";
        }
    }

    html += "	</span> ";
    html += "	<a href='#' onclick='goPage(" + nextPage + ")' class='next'><span class='blind'>다음페이지로 가기</span></a> ";
    html += "	<a href='#' onclick='goPage(" + totalPage + ")' class='next-last'><span class='blind'>마지막페이지로 가기</span></a> ";
    html += "</div> ";

    $(".paging-area").append(html);    // 페이지 목록 생성
}