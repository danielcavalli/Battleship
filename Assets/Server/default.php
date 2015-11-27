php

$servername = 'mysql.hostinger.com.br';
$username = 'u186988407_admin';
$password = 'battleship123';
$dbname = 'u186988407_bship';
$canSubscribe = false;

if($_POST[action] != null && $_POST[action] != )
{
   switch($_POST[action]){
    case Subscribe
    checkIfPlayerAlreadyExists();
    SubscribeNewPlayer();
    break;
   }
}

function checkIfPlayerAlreadyExists()
{
	$conn = new mysqli($servername, $username, $password, $dbname);
	if ($conn-connect_error) {
			die(Connection failed  . $conn-connect_error);
			 echo Didn't worked;
	} 

	$sql = SELECT Username FROM Players WHERE Username = .$_POST['username'].;;
	$result = $conn-query($sql);

		if ($result-num_rows == 0) {
			$canSubscribe = true;
		}
		else 
		echo There is someone already with that name;
		
	$conn-close();
}

function SubscribeNewPlayer()
{
	echo Started subscribing;
	$conn = new mysqli($servername, $username, $password, $dbname);
	if ($conn-connect_error) {
			die(Connection failed  . $conn-connect_error);
	echo Didn't worked;
	} 
	$sql = INSERT INTO u186988407_bship.Players(ID, Username, Hash,State,Status)
	VALUES (NULL, '.$_POST['username'].','. $_POST['hash'].','.$_POST['state'].','.$_POST['status'].');;

	if ($conn-query($sql) === TRUE) {
			echo New record created successfully;
	} else {
			echo Error  . $sql .. $conn-error;
	}
	$conn-close();
}


						