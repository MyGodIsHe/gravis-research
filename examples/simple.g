if_le = if[<=]
sub1 = opr[-]
sub2 = opr[-]
plus = opr[+]
const1 = const[1]
output1 = output

input >> if_le
const[1] >> if_le
if_le.true >> const1
const1 >> output1
if_le.false >> sub1
const[1] >> sub1
sub1 >> plus
if_le.false >> sub2
const[2] >> sub2
sub2 >> plus
plus >> output1
