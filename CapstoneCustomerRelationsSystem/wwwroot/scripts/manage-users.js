// Toggles the mobile navigation
ToggleMenu = function() {
	document.querySelector('.role-to-add-container').classList.toggle('hide');
}; //end ToggleMobileNav

// Populates the Selected User Account fields
PopulateUserAccountFields = function(_selectedUserAccountNumber, _selectedUserName) {
	//Set a value to the hidden field
	document.querySelector('#SelectedUserAccountNumber').value = _selectedUserAccountNumber;
	//Set a value to the span
	document.querySelector('#SelectedUserAccountName').innerHTML = _selectedUserName.toString();
}//end PopulateUserAccountFields

// Toggles the mobile navigation
ToggleRemoveMenu = function () {
	document.querySelector('.role-to-remove-container').classList.toggle('hide');
}; //end ToggleMobileNav

// Populates the Selected User Account fields
PopulateRemoveMenuFields = function (_selectedUserAccountNumber, _selectedUserName, _selectedRoleName) {
	//Set a value to the hidden field
	document.querySelector('#SelectedUserAccountNumberToRemove').value = _selectedUserAccountNumber;
	//Set a value to the hidden field
	document.querySelector('#SelectedRoleNameToRemove').value = _selectedRoleName;
	//Set a value to the span
	document.querySelector('#ViewRoleToRemoveName').innerHTML = _selectedRoleName;
	//Set a value to the span
	document.querySelector('#SelectedUserAccountNameToRemove').innerHTML = _selectedUserName.toString();
}//end PopulateUserAccountFields