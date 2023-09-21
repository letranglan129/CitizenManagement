$(() => {
    const drawer = $('#drawer').dxDrawer({
        opened: true,
        template() {
            const $list = $('<div>').width(200).addClass('panel-list');

            return $list.dxList({
                dataSource: navigation,
                hoverStateEnabled: false,
                focusStateEnabled: false,
                activeStateEnabled: false,
                elementAttr: { class: 'dx-theme-accent-as-background-color' },
                onItemClick(e) {
                    if (e.itemData.path != window.location.pathname)
                        window.location.href = e.itemData.path
                }
            });
        },
    }).dxDrawer('instance');

    const actionSheet = $('#action-sheet').dxActionSheet({
        dataSource: [
            { text: "Đổi mật khẩu" },
            { text: "Đăng xuất" },
        ],
        usePopover: true,
        elementAttr: {
            class: "actionSheetContainer"
        },
        showTitle: false,
        onItemClick(value) {
            console.log(value)
            if (value.itemData.text == "Đổi mật khẩu") {
                const popup = $('#accountChangePasswordPopup').dxPopup({
                    contentTemplate: () => {
                        let form = $('<div>').dxForm({
                            formData: {
                                currentPass: "",
                                newPass: "",
                                renewPass: "",
                            },
                            height: 'auto',
                            readOnly: false,
                            showColonAfterLabel: true,
                            labelLocation: 'left',
                            colCount: 2,
                            validationGroup: "validationChangePassGroup",
                            items: [
                                {
                                    dataField: 'currentPass',
                                    label: {
                                        text: "Mật khẩu hiện tại"
                                    },
                                    colCount: 2,
                                    colSpan: 2,
                                    editorOptions: {
                                        mode: 'password',
                                    },
                                    validationRules: [{
                                        type: 'required',
                                    }]
                                },
                                {
                                    dataField: 'newPass',
                                    label: {
                                        text: "Mật khẩu mới"
                                    },
                                    colCount: 2,
                                    colSpan: 2,
                                    editorOptions: {
                                        mode: 'password',
                                    },
                                    validationRules: [{
                                        type: 'required',
                                    }]
                                },
                                {
                                    dataField: 'renewPass',
                                    label: {
                                        text: "Nhập lại mật khẩu mới"
                                    },
                                    colCount: 2,
                                    colSpan: 2,
                                    editorOptions: {
                                        mode: 'password',
                                    },
                                    validationRules: [{
                                        type: 'required',
                                    }]
                                }, {
                                    itemType: 'group',
                                    colCount: 2,
                                    colSpan: 2,
                                    items: [{
                                        itemType: 'button',
                                        colSpan: 1,
                                        cssClass: 'text-center',
                                        buttonOptions: {
                                            icon: 'save',
                                            text: 'Lưu',
                                            onClick() {
                                                const data = form.dxForm('instance').option('formData');
                                                const x = DevExpress.validationEngine.validateGroup('validationChangePassGroup');

                                                if (x.isValid) {
                                                    axios({
                                                        url: `/Account/ChangePassword`,
                                                        method: 'POST',
                                                        data: data
                                                    }).then(() => {
                                                        toast.option({ message: 'Đổi mật khẩu thành công', type: 'success' });
                                                        toast.show();
                                                    }).catch(() => {
                                                        toast.option({ message: 'Đổi mật khẩu thất bại', type: 'error' });
                                                        toast.show();
                                                    })
                                                }
                                            },
                                        },
                                    }, {
                                        itemType: 'button',
                                        cssClass: 'text-center',
                                        colSpan: 1,
                                            buttonOptions: {
                                                icon: 'close',
                                                text: 'Đóng',
                                                onClick() {
                                                    console.log(popup.hide())
                                                },
                                            },
                                        },]
                                },
                            ]
                        })

                        return form
                    },
                    container: '.dx-viewport',
                    showTitle: true,
                    height: 'auto',
                    title: 'Đổi mật khẩu',
                    visible: false,
                    dragEnabled: false,
                    hideOnOutsideClick: true,
                    showCloseButton: false,
                    position: {
                        at: 'center',
                        my: 'center',
                        collision: 'fit',
                    },
                }).dxPopup('instance');
                popup.show();
            }
            if (value.itemData.text == "Đăng xuất") {
                window.location.href = '/Login/Logout'
            }
        },
    }).dxActionSheet('instance');

    $('#toolbar').dxToolbar({
        items: [{
            widget: 'dxButton',
            location: 'before',
            options: {
                icon: 'menu',
                onClick() {
                    drawer.toggle();
                },
            },
        }, {
                widget: 'dxButton',
                location: 'after',
                options: {
                    icon: 'group',
                    onClick(e) {
                        actionSheet.option('target', e.element[0]);
                        actionSheet.option('visible', true);
                    },
                },
            }],


    });

    $('#reveal-mode').dxRadioGroup({
        items: ['slide', 'expand'],
        layout: 'horizontal',
        value: 'slide',
        onValueChanged(e) {
            drawer.option('revealMode', e.value);
        },
    });

    $('#opened-state-mode').dxRadioGroup({
        items: ['push', 'shrink', 'overlap'],
        layout: 'horizontal',
        value: 'shrink',
        onValueChanged(e) {
            drawer.option('openedStateMode', e.value);
            $('#reveal-mode-option').css('visibility', e.value !== 'push' ? 'visible' : 'hidden');
        },
    });

    $('#position-mode').dxRadioGroup({
        items: ['left', 'right'],
        layout: 'horizontal',
        value: 'left',
        onValueChanged(e) {
            drawer.option('position', e.value);
        },
    });
});
