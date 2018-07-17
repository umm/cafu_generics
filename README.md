# CAFU Generics

* 汎用的な値を取り扱うためのモジュール
* 汎用的な Entity と、汎用的な状態を管理するための Model を提供する

## Requirement

* Unity 2017.1
* [CAFU Core v2](https://github.com/umm/cafu_core)

## Install

```shell
npm install github:umm/cafu_generics
```

## Usage

### GenericEntity

* 作成したいデータの構造に合わせて Entity を作成する
  * Unity の Serializer の制約上、Generics を用いたクラスはシリアライズできないため、 `GenericEntity<>` などを継承したクラスを作る必要がある
* 通常のクラス用と、ScriptableObject 用の2種類がある
  * 違いは、ScriptableObject を継承しているかどうかのみ

#### `GenericEntity<TValue>`, `ScriptableObjectGenericEntity<TValue>`

* 任意の型を1つだけ内包する Entity
* 定数的な要素や共通的に用いる Prefab を配置する場合などに用いる

#### `GenericPairEntity<TKey, TValue>`, `ScriptableObjectGenericPairEntity<TKey, TValue>`

* Key Value のペアを管理するための Entity
* 基本的には疑似 Dictionary の一要素としての定義を行う
  * `class HogeSpriteEntity : GenericPairEntity<HogeType, Sprite>` などと定義することで、 HogeType の値に応じた Sprite のマップの一要素を作ることができる

#### `GenericListEntity<TValue>`, `ScriptableObjectGenericListEntity<TValue>`

* 任意の型をリストとして内包する Entity
* ランダムに定数の一覧から値を取得する要件に対してマスタデータを定義する場合などに用いる

#### `GenericEntityList<TGenericEntity>`, `ScriptableObjectGenericEntityList<TGenericEntity>`

* GenricEntity をリストとして内包する Entity
  * 紛らわしいが、値の型制約として `IGenericEntity` 型に限定している
* 定数 Entity の一覧から値を取得するなどの場合に用いる
  * 一応用意しているが、あまり使うコトは無さそう
* `GetChildEntity(Func<TGenericEntity, bool> predicate)` という子要素を検索するための拡張メソッドも生やしてある

#### `GenericPairEntityList<TGenericPairEntity>`, `ScriptableObjectGenericPairEntityList<TGenericPairEntity>`

* GenericPairEntity をリストとして内包する Entity
  * 紛らわしいが、値の型制約として `IGenericPairEntity` 型に限定している
* 疑似 Dictionary を作る場合などに用いる
  * 恐らく最も利用する頻度の高い Entity
* `GetChildEntity(TKey key)` という子要素を検索するための拡張メソッドも生やしてある

#### `GenericDataStore`

* Entity を保持管理するためのクラス
* Scene 上の任意の GameObject に AddComponent し、シーン内で読み込みたい Entity を登録していく
  * GameObject 名は何でも良いが `DataStore` としておくと見通しが良い
  * `Scriptable Object Generic Entity List` というフィールドが Inspector 上に生えているので、そこに ScriptableObject を D&amp;D する
* `GetEntity<TGenericEntity>()` メソッドで Entity のインスタンスを返す

#### `GenericRepository<TGenericEntity>`

* 型引数に渡した Entity を GenericDataStore から取得する
* 任意の UseCase で、このインスタンスを必要な Entity の数分生成し、 Model の構成要素として Entity を取得する
* `GetEntity()` メソッドで Entity のインスタンスを返す

#### `GenericUseCase<TGenericEntity>`

* Presenter から直接 Entity を参照したい場合に用いる
* CAFU 的には想定していない操作になるため、原則非推奨
* `GetEntity()` メソッドで Entity のインスタンスを返す

#### Generator

* ScriptableObject のアセットファイルを作成するためのカスタムエディタウィンドウを提供
* メニューの Window &gt; CAFU &gt; Entity Generator からウィンドウを開く
* `ScriptableObjectGenericEntity` を継承したクラスが作成対象

<img width="407" alt="screenshot 2018-02-15 12 51 09" src="https://user-images.githubusercontent.com/838945/36239702-09d5d23a-124f-11e8-808a-17e6ff5aa698.png">
<img width="434" alt="screenshot 2018-02-15 12 51 17" src="https://user-images.githubusercontent.com/838945/36239703-0cae942e-124f-11e8-8301-39a5d7b411a0.png">

### GenericStateModel

#### `GenericStateModel<TState>`

* **状態**を管理するための Model クラス
* 型引数の制約として `struct` としているので、enum を用いた状態遷移や bool によるトグルなどをサポートする
* 任意の UseCase からの利用を想定して、備えるべきメソッドとその実装を内包している

##### 公開メソッド

| Method Signature | Description |
| --- | --- |
| `TState GetCurrent()` | 現在の値を取得 |
| `void Change(TState state, bool forceNotify = false)` | 状態を変更<br />第二引数に真を渡すと、値が変更されていなくても `OnChangeAsObservable` に値を流す（内部的には ReactiveProperty.SetValueAndForceNotify）を実行している |
| `void Reset()` | 値を初期値に戻す |
| `void Next()` | 値を一つ進める<br />`dynamic` による加算を行っているため、 `+` オペレータを解釈できない型の場合 Exception を吐く |
| `void Previous()` | 値を一つ戻す<br />`dynamic` による減算を行っているため、 `-` オペレータを解釈できない型の場合 Exception を吐く |
| `IObservable<TState> OnChangeAsObservable()` | 値の変更を通知するストリームを構築 |
| `IObservable<Unit> OnChangeAsObservable(TState state)` | 値が第一引数の値になったコトを通知するストリームを構築 |

#### `GenericStateUseCase<TState>`

* `GenericStateModel<TState>` の UseCase Wrapper
* Presenter レイヤから取り扱えるように用意している
* 公開メソッドは `GenericStateModel<TState>` と等しい

## License

Copyright (c) 2018 Tetsuya Mori

Released under the MIT license, see [LICENSE.txt](LICENSE.txt)
