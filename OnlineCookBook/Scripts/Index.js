$('.specific-category').on('click', function (e) {


     window.location.href = "/Home/RecepiesView" + '?type=' + e.currentTarget.id;
    //var url= '@Url.Action("RecepiesView", "Home",new { type = e.currentTarget.id })';
    //window.location = url;


});

function searchByTags(e) {
    if (e.keyCode == 13) {
        var filter = $("#searchInput").val();
        var filters = filter.split(' ');
        $.ajax({
            url: '/Home/RecepiesView',
            type: "POST",
            dataType: "json",
            data: JSON.stringify(filters),
            contentType: 'application/json; charset=utf-8',
        }).success(function (data) {
            location = data.url;
        });
    
        
    }
}