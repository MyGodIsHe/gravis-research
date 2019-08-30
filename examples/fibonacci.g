fibonacci = subspace [
    if_le = if[<=]
    sub_left = opr[-]
    sub_right = opr[-]
    plus = opr[+]
    const1 = const[1]
    output1 = output
    self_left = subspace[self]
    self_right = subspace[self]

    input >> if_le
    const[2] >> if_le
    if_le.true >> const1
    const1 >> output1
    if_le.false >> sub_left
    const[1] >> sub_left
    sub_left >> self_left
    self_left >> plus
    if_le.false >> sub_right
    const[2] >> sub_right
    sub_right >> self_right
    self_right >> plus
    plus >> output1
]
input >> fibonacci
fibonacci >> output
