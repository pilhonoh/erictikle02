_PagingController = function () { };
_PagingController.prototype =
{
    Id: '',
    AlwaysButton: true,
    FunctionName: '',
    TotalCount: 0,
    PageNumber: 1,
    PageSize: 15,
    BlockSize: 10,
    UseActiveLink: true,
    //UseFirst: true,
    //UseLast: true,
    //UsePrev: true,
    //UseNext: true,
    Initialize: function () {
    },
    Append: function (totalCount) {
        var sb = new Array();

        if (totalCount == 0) {
            $("#" + this.Id).empty();
            $("#" + this.Id).append(sb.join(''));
        }
        else {
            var totalpageCount = (parseInt(totalCount) % parseInt(this.PageSize)) == 0 ? parseInt(totalCount / this.PageSize) : parseInt(totalCount / this.PageSize) + 1;
            if (totalpageCount <= 0) {
                totalpageCount = 1;
            }

            var currentBlock = (parseInt(this.PageNumber) % parseInt(this.BlockSize)) == 0 ? parseInt(this.PageNumber / this.BlockSize) : parseInt(this.PageNumber / this.BlockSize) + 1;
            var lastBlock = (parseInt(totalpageCount) % parseInt(this.BlockSize)) == 0 ? parseInt(totalpageCount / this.BlockSize) : parseInt(totalpageCount / this.BlockSize) + 1;
            var firstPage = (currentBlock - 1) * this.BlockSize + 1;
            var lastPage = (currentBlock * this.BlockSize) > totalpageCount ? totalpageCount : currentBlock * this.BlockSize;

            
            if (currentBlock > 1 || this.AlwaysButton) {
                sb.push('<a href="javascript:;" onclick="' + this.FunctionName + '(1);" class="leftEnd">&nbsp;</a>');//처음으로

                if (currentBlock > 1) {
                    sb.push('<a href="javascript:;" onclick="' + this.FunctionName + '(' + (firstPage - this.BlockSize).toString() + '); return false;" class="leftMove">&nbsp;</a>');//이전으로
                }
                else {
                    sb.push('<a href="javascript:;" onclick="' + this.FunctionName + '(' + firstPage.toString() + '); return false;" class="leftMove">&nbsp;</a>');
                }
            }
            for (var i = firstPage; i <= lastPage; i++) {
                if (i == this.PageNumber && this.UseActiveLink) {
                    sb.push('<a href="javascript:;" onclick="' + this.FunctionName + '(' + i.toString() + '); return false;" class="selected" style="margin-left:2px;">' + i + '</a>');

                }
                else {
                   
                    sb.push('<a href="javascript:;" onclick="' + this.FunctionName + '(' + i.toString() + '); return false;" style="margin-left:2px;">' + i + '</a>');
                }
            }

            if (currentBlock < lastBlock || this.AlwaysButton) {
                var nextPage = lastPage + 1;
                if (totalpageCount < nextPage)
                    nextPage = totalpageCount;

                sb.push('<a href="javascript:;" onclick="' + this.FunctionName + '(' + nextPage.toString() + '); return false;" class="rightMove">&nbsp;</a>');//다음으로
                sb.push('<a href="javascript:;" onclick="' + this.FunctionName + '(' + totalpageCount.toString() + '); return false;" class="rightEnd">&nbsp;</a>');//마지막으로
            }

            $("#" + this.Id).empty();
            $("#" + this.Id).append(sb.join(''));
        }
    }
};
var PagingController = new _PagingController();



_PagingController1 = function () { };
_PagingController1.prototype =
    {
        Id: '',
        AlwaysButton: true,
        FunctionName: '',
        TotalCount: 0,
        PageNumber: 1,
        PageSize: 15,
        BlockSize: 10,
        UseActiveLink: true,
        //UseFirst: true,
        //UseLast: true,
        //UsePrev: true,
        //UseNext: true,
        Initialize: function () {
        },
        Append: function (totalCount) {
            var sb = new Array();

            if (totalCount == 0) {
                $("#" + this.Id).empty();
                $("#" + this.Id).append(sb.join(''));
            }
            else {
                var totalpageCount = (parseInt(totalCount) % parseInt(this.PageSize)) == 0 ? parseInt(totalCount / this.PageSize) : parseInt(totalCount / this.PageSize) + 1;
                if (totalpageCount <= 0) {
                    totalpageCount = 1;
                }

                var currentBlock = (parseInt(this.PageNumber) % parseInt(this.BlockSize)) == 0 ? parseInt(this.PageNumber / this.BlockSize) : parseInt(this.PageNumber / this.BlockSize) + 1;
                var lastBlock = (parseInt(totalpageCount) % parseInt(this.BlockSize)) == 0 ? parseInt(totalpageCount / this.BlockSize) : parseInt(totalpageCount / this.BlockSize) + 1;
                var firstPage = (currentBlock - 1) * this.BlockSize + 1;
                var lastPage = (currentBlock * this.BlockSize) > totalpageCount ? totalpageCount : currentBlock * this.BlockSize;


                if (currentBlock > 1 || this.AlwaysButton) {
                    sb.push('<a href="javascript:;" onclick="' + this.FunctionName + '(1);" class="leftEnd">&nbsp;</a>');//처음으로

                    if (currentBlock > 1) {
                        sb.push('<a href="javascript:;" onclick="' + this.FunctionName + '(' + (firstPage - this.BlockSize).toString() + '); return false;" class="leftMove">&nbsp;</a>');//이전으로
                    }
                    else {
                        sb.push('<a href="javascript:;" onclick="' + this.FunctionName + '(' + firstPage.toString() + '); return false;" class="leftMove">&nbsp;</a>');
                    }
                }
                for (var i = firstPage; i <= lastPage; i++) {
                    if (i == this.PageNumber && this.UseActiveLink) {
                        sb.push('<a href="javascript:;" onclick="' + this.FunctionName + '(' + i.toString() + '); return false;" class="selected" style="margin-left:2px;">' + i + '</a>');

                    }
                    else {

                        sb.push('<a href="javascript:;" onclick="' + this.FunctionName + '(' + i.toString() + '); return false;" style="margin-left:2px;">' + i + '</a>');
                    }
                }

                if (currentBlock < lastBlock || this.AlwaysButton) {
                    var nextPage = lastPage + 1;
                    if (totalpageCount < nextPage)
                        nextPage = totalpageCount;

                    sb.push('<a href="javascript:;" onclick="' + this.FunctionName + '(' + nextPage.toString() + '); return false;" class="rightMove">&nbsp;</a>');//다음으로
                    sb.push('<a href="javascript:;" onclick="' + this.FunctionName + '(' + totalpageCount.toString() + '); return false;" class="rightEnd">&nbsp;</a>');//마지막으로
                }

                $("#" + this.Id).empty();
                $("#" + this.Id).append(sb.join(''));
            }
        }
    };
var PagingController1 = new _PagingController1();

_PagingController2 = function () { };
_PagingController2.prototype =
    {
        Id: '',
        AlwaysButton: true,
        FunctionName: '',
        TotalCount: 0,
        PageNumber: 1,
        PageSize: 15,
        BlockSize: 10,
        UseActiveLink: true,
        //UseFirst: true,
        //UseLast: true,
        //UsePrev: true,
        //UseNext: true,
        Initialize: function () {
        },
        Append: function (totalCount) {
            var sb = new Array();

            if (totalCount == 0) {
                $("#" + this.Id).empty();
                $("#" + this.Id).append(sb.join(''));
            }
            else {
                var totalpageCount = (parseInt(totalCount) % parseInt(this.PageSize)) == 0 ? parseInt(totalCount / this.PageSize) : parseInt(totalCount / this.PageSize) + 1;
                if (totalpageCount <= 0) {
                    totalpageCount = 1;
                }

                var currentBlock = (parseInt(this.PageNumber) % parseInt(this.BlockSize)) == 0 ? parseInt(this.PageNumber / this.BlockSize) : parseInt(this.PageNumber / this.BlockSize) + 1;
                var lastBlock = (parseInt(totalpageCount) % parseInt(this.BlockSize)) == 0 ? parseInt(totalpageCount / this.BlockSize) : parseInt(totalpageCount / this.BlockSize) + 1;
                var firstPage = (currentBlock - 1) * this.BlockSize + 1;
                var lastPage = (currentBlock * this.BlockSize) > totalpageCount ? totalpageCount : currentBlock * this.BlockSize;


                if (currentBlock > 1 || this.AlwaysButton) {
                    sb.push('<a href="javascript:;" onclick="' + this.FunctionName + '(1);" class="leftEnd">&nbsp;</a>');//처음으로

                    if (currentBlock > 1) {
                        sb.push('<a href="javascript:;" onclick="' + this.FunctionName + '(' + (firstPage - this.BlockSize).toString() + '); return false;" class="leftMove">&nbsp;</a>');//이전으로
                    }
                    else {
                        sb.push('<a href="javascript:;" onclick="' + this.FunctionName + '(' + firstPage.toString() + '); return false;" class="leftMove">&nbsp;</a>');
                    }
                }
                for (var i = firstPage; i <= lastPage; i++) {
                    if (i == this.PageNumber && this.UseActiveLink) {
                        sb.push('<a href="javascript:;" onclick="' + this.FunctionName + '(' + i.toString() + '); return false;" class="selected" style="margin-left:2px;">' + i + '</a>');

                    }
                    else {

                        sb.push('<a href="javascript:;" onclick="' + this.FunctionName + '(' + i.toString() + '); return false;" style="margin-left:2px;">' + i + '</a>');
                    }
                }

                if (currentBlock < lastBlock || this.AlwaysButton) {
                    var nextPage = lastPage + 1;
                    if (totalpageCount < nextPage)
                        nextPage = totalpageCount;

                    sb.push('<a href="javascript:;" onclick="' + this.FunctionName + '(' + nextPage.toString() + '); return false;" class="rightMove">&nbsp;</a>');//다음으로
                    sb.push('<a href="javascript:;" onclick="' + this.FunctionName + '(' + totalpageCount.toString() + '); return false;" class="rightEnd">&nbsp;</a>');//마지막으로
                }

                $("#" + this.Id).empty();
                $("#" + this.Id).append(sb.join(''));
            }
        }
    };
var PagingController2 = new _PagingController2();