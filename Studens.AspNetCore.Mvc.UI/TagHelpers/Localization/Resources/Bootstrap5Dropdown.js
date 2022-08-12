$(function () {
	var $languageGroups = $("div[id^='language-control-']");

	$languageGroups.find("ul.dropdown-menu li").on('click', function () {
		var _lang = this.lang;
		var _flagIcon = $(this).data('langFlagIcon');

		// Set all form language switch buttons to
		$languageGroups.find("button.dropdown-toggle").html(`<span class='fi fi-${_flagIcon} fis'></span>`);

		// Find input for selected lang and show it, and hide other inputs
		var _$targetInputs = $languageGroups.find(`textarea:lang(${_lang}), input:lang(${_lang})`);
		_$targetInputs.each(function (_) {
			if ($(this).is('input')) {
				$(this).attr('type', 'text');
			} else {
				$(this).removeAttr('hidden'); //Textarea
			}
		});

		$languageGroups
			.find('textarea,input')
			.not(_$targetInputs)
			.each(function (_) {
				if ($(this).is('input')) {
					$(this).attr('type', 'hidden');
				} else {
					$(this).attr('hidden', true); //Textarea
				}
			});
	});
});