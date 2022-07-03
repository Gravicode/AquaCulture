function fw_open(url) {
    window.location = url;
}

function fw_alert(content){
    var dialog = $('#fwDialogModal');
    if (dialog.length>0) dialog.remove();
    dialog = $('<div class="modal fade" id="fwDialogModal" tabindex="-1" role="dialog" aria-labelledby="exampleModalCenterTitle" aria-hidden="true"><div class="modal-dialog modal-dialog-centered" role="document"><div class="modal-content"><div class="modal-header"><h5 class="modal-title" id="exampleModalLongTitle">Modal title</h5><button type="button" class="close" data-dismiss="modal" aria-label="Close"><span aria-hidden="true">&times;</span></button></div><div class="modal-body">...</div><div class="modal-footer"><button type="button" class="btn btn-primary px-5" data-dismiss="modal"> Ok </button></div></div></div></div>').appendTo('body');
    $('.modal-body',dialog).text(content);
    dialog.modal({});
}

function fw_initPage(){
    var ww = $(window).width();
    var dialog = $('#fwDialogModal');
    if (dialog.length==0) {
        dialog = $('<div class="modal fade" id="fwDialogModal" tabindex="-1" role="dialog" aria-labelledby="exampleModalCenterTitle" aria-hidden="true"><div class="modal-dialog modal-dialog-centered" role="document"><div class="modal-content"><div class="modal-header"><h5 class="modal-title" id="exampleModalLongTitle">Modal title</h5><button type="button" class="close" data-dismiss="modal" aria-label="Close"><span aria-hidden="true">&times;</span></button></div><div class="modal-body">...</div><div class="modal-footer"><button type="button" class="btn btn-secondary" data-dismiss="modal">Close</button><button type="button" class="btn btn-primary">Save changes</button></div></div></div></div>').appendTo('body');
    }
    $('.table-responsive').each(function(){
        var p=$(this).parent();
        if (!p.is('.table-container')) {
            $(this).wrap('<div class="table-container"></div>');
        }
    });
    $('.table-responsive').on('scroll',function(){
        var t = $(this), sw = t[0].scrollWidth, w = t.width(), sl = t.scrollLeft(), dw = sw-w, p = t.closest('.table-container');
        console.log(Math.round(sw)+';;'+Math.round(w));
        if (p.length>0 && Math.round(sw)>Math.round(w)){
            if (sl == 0) {
                p.removeClass('darkLeft').addClass('darkRight');
            } else if (sl > 0 && sl<dw) {
                p.addClass('darkLeft darkRight');
            } else if (sl == dw ) {
                p.removeClass('darkRight').addClass('darkLeft');
            }
        }
    }).trigger('scroll');
    //if (ww<=576){
        var ld = $('[data-listFromId]');
        var clickMenuItem = function(obj) {
            var o = $(obj), p = o.closest('[data-listFromId]'), tgt = $('#'+p.attr('data-listFromId')), v = o.attr('data-value');
            $('option[value="'+v+'"]',tgt).prop('selected',true);
            tgt.trigger('change');
            $('a.active',p).removeClass('active');
            o.addClass('active');
        };
        if (ld.length>0) {
            ld.each(function(){
                var t= $(this).empty(), lid=$(this).attr('data-listFromId'), o = $('#'+lid);
                if (o.length>0){
                    var p = o.closest('.form-group'), lbl = $('label',p).text(), opt = $('option',o), dv = o.val();
                    t.append('<h6>'+lbl+'</h6><div class="nav-links"></div>');
                    var l = $('.nav-links',t);
                    for(var x=0; x<opt.length; x++) {
                        if (typeof $(opt[x]).attr('value') != 'undefined') {
                            var lnk = $('<a class="nav-link'+(dv == $(opt[x]).attr('value')?' active':'')+'" href="#" data-value="'+$(opt[x]).attr('value')+'">'+$(opt[x]).text()+'</a>');
                            lnk.on('click',function(e){
                                e.preventDefault();
                                e.stopPropagation();
                                clickMenuItem(this);
                            })
                            l.append(lnk);
                        }
                    }
                } 

            });

        }
    //}

    // check all
    $('.fw-checkAll').on('click',function(){
        var tr = $(this).closest('tr'), th = $(this).closest('th'), ith = $('th',tr).index(th), tb= $(this).closest('table'), stat = $(this).prop('checked');
        $('tbody>tr',tb).each(function(){
            $('td',this).eq(ith).find('input[type="checkbox"]').prop('checked',stat);
        });
    });

    $(window).on('resize',fw_initResize);
    fw_initResize();

    // relocate block
    if ($('.navbar-toggler').is(':visible')) {
        $('.mobile-relocation[data-fromId]').each(function(){
            var t = $(this), tgt = $('#'+t.attr('data-fromId'));
            if (tgt.length>0) {
                tgt.appendTo(t);
            }
        });
    }
}

function fw_initResize(){
    var w = $(window).width();
    if (w>=1349) {
        var ratio = (w/1349).toFixed(2);
        $('body.fw-page>.fw-container, .modal>.modal-dialog').css({
            transform : 'scale('+ratio+')'
        });
    }
}

$(function(){
    fw_initPage();
});