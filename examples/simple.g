if_le = If(<=)
sub1 = Operator(-)
sub2 = Operator(-)
plus = Operator(+)
const1 = Const(1)
output = Output()

Input() >> if_le
Const(1) >> if_le
if_le.true >> const1
const1 >> output
if_le.false >> sub1
Const(1) >> sub1
sub1 >> plus
if_le.false >> sub2
Const(2) >> sub2
sub2 >> plus
plus >> output
